using System.Collections.Generic;
using System.Linq;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.DTO;

namespace DAL.Utils
{
    public static class PlayerBoardProvider
    {
        public static ECellState[,] GetEnemyBoard(List<BoatDal> enemyBoats, List<LocationPointDal> playerHits, GameSettingsDal gc)
        {
            var generatedBoard = GenerateEmptyBoard(gc);
            
            foreach (var locationPoint in enemyBoats.SelectMany(boat => boat.Locations))
            {
                generatedBoard[locationPoint.X, locationPoint.Y] = ECellState.Ship;
            }
            
            foreach (var hit in playerHits)
            {
                generatedBoard[hit.X, hit.Y] = hit.PointState;
            }

            return generatedBoard;
        }
        
        public static ECellState[,] GetAttackerBoard(List<BoatDal> attackerBoats, List<LocationPointDal> enemyHits, GameSettingsDal gc)
        {
            var generatedBoard = GenerateEmptyBoard(gc);
            
            foreach (var locationPoint in attackerBoats.SelectMany(boat => boat.Locations))
            {
                generatedBoard[locationPoint.X, locationPoint.Y] = ECellState.Ship;
            }
            
            foreach (var hit in enemyHits)
            {
                generatedBoard[hit.X, hit.Y] = hit.PointState;
            }

            return generatedBoard;
        }
        
        private static ECellState[,] GenerateEmptyBoard(GameSettingsDal gs)
        {
            var board = new ECellState[
                gs.FieldHeight,
                gs.FieldWidth
            ];

            for (var i = 0; i < board.GetUpperBound(1); i++)
            {
                for (var j = 0; j < board.GetUpperBound(0); j++)
                {
                    board[i, j] = ECellState.Empty;
                }
            }

            return board;
        }
        
        private static ECellState[,] GenerateEmptyBoard(GameSettingsDto gs)
        {
            var board = new ECellState[
                gs.FieldHeight,
                gs.FieldWidth
            ];

            for (var i = 0; i < board.GetUpperBound(1); i++)
            {
                for (var j = 0; j < board.GetUpperBound(0); j++)
                {
                    board[i, j] = ECellState.Empty;
                }
            }

            return board;
        }

        public static ECellState[,] GetPlayerBoardWithBoats(List<BoatDto> boats, GameSettingsDto gc)
        {
            var generatedBoard = GenerateEmptyBoard(gc);
            
            foreach (var locationPoint in boats.SelectMany(boat => boat.Locations!))
            {
                generatedBoard[locationPoint.X, locationPoint.Y] = ECellState.Ship;
            }

            return generatedBoard;
        }
    }
}
