using System;
using DAL.DAL.Entities;
using DAL.DTO;

namespace DAL.Mappers
{
    public class GameEngineMapper
    {
        public static GameEngine MapToDal(GameEngineDto gameEngineDto)
        {
            var gameSettings = new GameSettingsDal
            {
                FieldHeight = gameEngineDto.GameSettings.FieldHeight,
                FieldWidth = gameEngineDto.GameSettings.FieldWidth,
                HitContinuousMove = gameEngineDto.GameSettings.HitContinuousMove,
                BoatsCanTouch = gameEngineDto.GameSettings.BoatsCanTouch,
                BoatsConfig = gameEngineDto.GameSettings.BoatsConfig
            };

            var historyManager = HitHistoryManagerMapper.MapToDal(gameEngineDto.HitHistoryManager);

            var playerA = PlayerMapper.MapToDal(gameEngineDto.GetFirstPlayer());

            var playerB = PlayerMapper.MapToDal(gameEngineDto.GetSecondPlayer());

            var nextMoveByFirst = gameEngineDto.NextMoveByFirst;
            
            if (gameEngineDto.SessionId.HasValue)
            {
                return new GameEngine(
                    gameSettings, 
                    historyManager, 
                    playerA, 
                    playerB, 
                    nextMoveByFirst, 
                    gameEngineDto.SessionId.Value);
            }

            if (!string.IsNullOrEmpty(gameEngineDto.SaveFilePath))
            {
                return new GameEngine(
                    gameSettings, 
                    historyManager, 
                    playerA, 
                    playerB, 
                    nextMoveByFirst, 
                    gameEngineDto.SaveFilePath);
            }

            return new GameEngine(gameSettings, playerA, playerB);
        }
    }
}