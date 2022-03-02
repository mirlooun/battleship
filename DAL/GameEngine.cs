using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.DAL.Entities.Enums.Rules;
using DAL.DTO;
using DAL.Helpers;
using DAL.Mappers;
using DAL.Services;
using DAL.Utils;

namespace DAL
{
    public class GameEngine
    {
        private readonly PlayerHolder _playerHolder;

        private readonly GameSettingsDal _gameSettings;

        private readonly HitHistoryManager _historyManager;
        public bool NextMoveByFirst { get; set; }
        private bool IsNextMoveByFirst() => NextMoveByFirst;

        private (int? SessionId, string? SaveFilePath, bool IsFinished) _sessionInfo;
        public GameEngine(GameSettingsDal gs, PlayerDal first, PlayerDal second)
        {
            _gameSettings = gs;
            NextMoveByFirst = true;
            _playerHolder = new PlayerHolder(first, second, IsNextMoveByFirst);
            _historyManager = new HitHistoryManager();
        }
        public GameEngine(GameSettingsDal gs, HitHistoryManager hitHistoryManager, PlayerDal first, PlayerDal second,
            bool nextMoveByFirst, string? saveFilePath)
        {
            _gameSettings = gs;
            NextMoveByFirst = nextMoveByFirst;
            _playerHolder = new PlayerHolder(first, second, IsNextMoveByFirst);
            _sessionInfo.SaveFilePath = saveFilePath;
            _historyManager = hitHistoryManager;
        }
        public GameEngine(GameSettingsDal gs, HitHistoryManager hitHistoryManager, PlayerDal first, PlayerDal second, 
            bool nextMoveByFirst, int sessionId)
        {
            _gameSettings = gs;
            NextMoveByFirst = nextMoveByFirst;
            _playerHolder = new PlayerHolder(first, second, IsNextMoveByFirst);
            _sessionInfo.SessionId = sessionId;
            _historyManager = hitHistoryManager;
        }
        public HitResponse MakeAHit(LocationDal hit)
        {
            var currentAttackingPlayer = _playerHolder
                .GetCurrentAttackingPlayer();
            
            var attempt = currentAttackingPlayer
                .GetHits()
                .Find(p => p.Equals(hit));

            // Player hits same cell
            if (attempt != null)
            {
                return new HitResponse(ECellState.Hit);
            }

            var currentEnemy = _playerHolder.GetCurrentEnemy();

            var targetBoat = TargetBoatFinder.FindBoat(
                currentEnemy.GetBoats(), hit);

            
            LocationPointDal hitLocation;
            // If player didn't hit any boat
            if (targetBoat == null)
            {
                hitLocation = new LocationPointDal(hit.X, hit.Y, ECellState.Miss);
                currentAttackingPlayer
                    .AddHitToHistory(hitLocation);
                
                _historyManager.AddHitToHistory(new HitRecord
                {
                    HitLocation = hitLocation,
                    IsFirst = NextMoveByFirst,
                    HitType = ECellState.Miss,
                    Info = GetHitInfo(
                        currentAttackingPlayer.Name,
                        currentEnemy.Name,
                        targetBoat
                        )
                });
                
                ChangeNextMoveBy();
                
                return new HitResponse(ECellState.Miss);
            }

            hitLocation = new LocationPointDal(hit.X, hit.Y, ECellState.Hit);
            // If player hits a boat
            targetBoat.MakeAHit(hitLocation);

            currentAttackingPlayer
                .AddHitToHistory(hitLocation);
            
            _historyManager.AddHitToHistory(new HitRecord
            {
                HitLocation = hitLocation,
                IsFirst = NextMoveByFirst,
                HitType = ECellState.Hit,
                Info = GetHitInfo(currentAttackingPlayer.Name, currentEnemy.Name, targetBoat)
            });
            
            if (_gameSettings.HitContinuousMove == EHitContinuousMove.HitDoesntContinueMove)
            {
                ChangeNextMoveBy();
            }

            return new HitResponse(targetBoat.GetName(), targetBoat.GetHp());
        }

        private static string GetHitInfo(string currentAttackingPlayerName, string currentEnemyName, BoatDal? targetBoat)
        {
            if (targetBoat == null)
            {
                return
                    $"{DateTime.Now} - Player {currentAttackingPlayerName} attacks player {currentEnemyName} and misses";
            }

            return targetBoat.GetHp() == 0 ?
                $"{DateTime.Now} - Player {currentAttackingPlayerName} attacks player {currentEnemyName} and" +
                $" destroys a {BoatTypeProvider.GetUiName(targetBoat.Type)}" 
                : $"{DateTime.Now} - Player {currentAttackingPlayerName} attacks player {currentEnemyName} and" +
                  " hits a boat";
        }

        private void ChangeNextMoveBy()
        {
            NextMoveByFirst = !NextMoveByFirst;
        } 
        public string GetCurrentAttackerName()
        {
            return _playerHolder.GetCurrentAttackingPlayer().Name;
        }
        public string GetCurrentEnemyName()
        {
            return _playerHolder.GetCurrentEnemy().Name;
        }

        public (ECellState[,] enemyBoard, ECellState[,] attackerBoard) GetBoards()
        {
            return (GetCurrentEnemyBoardState(), GetCurrentAttackerBoardState());
        }
        public ECellState[,] GetCurrentEnemyBoardState()
        {
            var enemyBoats = _playerHolder
                .GetCurrentEnemy()
                .GetBoats();
            var playerHits = _playerHolder
                .GetCurrentAttackingPlayer()
                .GetHits();
            return PlayerBoardProvider.GetEnemyBoard(enemyBoats, playerHits, _gameSettings);
        }
        
        private ECellState[,] GetCurrentAttackerBoardState()
        {
            var enemyHits = _playerHolder
                .GetCurrentEnemy()
                .GetHits();
            var playerBoats = _playerHolder
                .GetCurrentAttackingPlayer()
                .GetBoats();
            return PlayerBoardProvider.GetAttackerBoard(playerBoats, enemyHits, _gameSettings);
        }

        public (PlayerDal p1, PlayerDal p2) GetPlayers()
        {
            var players = _playerHolder.GetPlayers();
            return (players[0], players[1]);
        }
        public GameEngineDto GetGameStateDto()
        {
            var gameEngineDto = new GameEngineDto
            {
                SessionId = _sessionInfo.SessionId,
                SaveFilePath = _sessionInfo.SaveFilePath,
                IsFinished = _sessionInfo.IsFinished,
                Players = _playerHolder.GetPlayerDtoList(),
                NextMoveByFirst = NextMoveByFirst,
                GameSettings = GameSettingsMapper.MapToDto(_gameSettings),
                HitHistoryManager = HitHistoryManagerMapper.MapToDto(_historyManager),
            };
            return gameEngineDto;
        }
        public Stack<HitRecord> GetHitHistory()
        {
            return _historyManager.GetHistory();
        }
        public void RevertMovesToSpecified(HitRecord record)
        {
            _historyManager.RevertGameStateToSpecified(this, record);
        }
        public bool HasSessionId()
        {
            return !string.IsNullOrEmpty(_sessionInfo.SessionId.ToString());
        }
        public bool HasSaveFilePath()
        {
            return !string.IsNullOrEmpty(_sessionInfo.SaveFilePath);
        }

        public void SetSessionInfo(string path)
        {
            _sessionInfo.SaveFilePath = path;
        }

        public bool HasWinner()
        {
            var hasWinner = WinnerCalculator.HasWinner(_playerHolder.GetPlayers());

            if (hasWinner)
            {
                _sessionInfo.IsFinished = true;
            }
            
            return hasWinner;
        }

        public (string winner, string loser) GetWinnerAndLoser()
        {
            return WinnerCalculator.GetWinnerAndLooser(_playerHolder.GetPlayers());
        }

        public GameSettingsDal GetGameSettings()
        {
            return _gameSettings;
        }
    }
    internal static class WinnerCalculator
    {
        public static bool HasWinner(List<PlayerDal> players)
        {
            var playerABoatsHitPoints = CalculateBoatsHitPoints(players[0].GetBoats());
            var playerBBoatsHitPoints = CalculateBoatsHitPoints(players[1].GetBoats());
            return playerABoatsHitPoints == 0 || playerBBoatsHitPoints == 0;
        }
        private static int CalculateBoatsHitPoints(IEnumerable<BoatDal> boats)
        {
            return boats.Sum(boat => boat.GetHp());
        }
        public static (string winner, string loser) GetWinnerAndLooser(List<PlayerDal> players)
        {
            var playerABoatsHitPoints = CalculateBoatsHitPoints(players[0].GetBoats());
            
            return playerABoatsHitPoints == 0 ? (players[1].Name, players[0].Name) : (players[0].Name, players[1].Name);
        }
    }

    internal class PlayerHolder
    {
        private List<PlayerDal> Players { get; } = new(2);

        private readonly Func<bool> _isNextMoveByFirst;
        public PlayerHolder(PlayerDal first, PlayerDal second, Func<bool> isNextMoveByFirst)
        {
            _isNextMoveByFirst = isNextMoveByFirst;
            Players.AddRange(new List<PlayerDal>
            {
                first, second
            });
        }
        public PlayerDal GetCurrentAttackingPlayer()
        {
            var playerIndex = _isNextMoveByFirst() ? 0 : 1;
            return Players[playerIndex];
        }
        public PlayerDal GetCurrentEnemy()
        {
            var playerIndex = !_isNextMoveByFirst() ? 0 : 1;
            return Players[playerIndex];
        }
        public List<PlayerDto> GetPlayerDtoList()
        {
            return new List<PlayerDto>
            {
                PlayerMapper.MapToDto(Players[0]),
                PlayerMapper.MapToDto(Players[1])
            };
        }
        public (string p1, string p2) GetPlayerNamesInOrder()
        {
            return (Players[0].Name, Players[1].Name);
        }

        public List<PlayerDal> GetPlayers()
        {
            return Players;
        }
    }
}
