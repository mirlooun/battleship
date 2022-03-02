using System;
using System.Linq;
using Contracts.Validator;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.DAL.Entities.Enums.Rules;

namespace GameSetupUiProviders.Helpers
{
    public static class BoatLocationValidator
    {
        public static BoatLocationValidatorResponse IsBoatLocationOccupied(ECellState[,] board,
            BoatDal boat, EBoatsCanTouch rule)
        {
            var response = new BoatLocationValidatorResponse();
            
            switch (rule)
            {
                case EBoatsCanTouch.BoatsCanTouch:
                    response.IsValid = boat.Locations.All(point => board[point.X, point.Y] != ECellState.Ship);
                    
                    if (!response.IsValid)
                    {
                        response.Message = "Cells are already occupied by another boat!";
                    }
                    
                    return response;
                case EBoatsCanTouch.BoatsCantTouch:
                    response.IsValid = boat.Locations.All(point => board[point.X, point.Y] != ECellState.Ship); 
                        
                    if (!response.IsValid)
                    {
                        response.Message = "Cells are already occupied by another boat!";
                        return response;
                    }
                    
                    response.IsValid = boat.Locations.All(point => !IsOccupiedAround(board, point));

                    if (!response.IsValid)
                    {
                        response.Message = "You can't place a boat near to another boat!";
                    }
                    
                    return response;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rule), rule, null);
            }
        }

        private static bool IsOccupiedAround(ECellState[,] board, LocationDal point)
        {
            for (var i = point.X - 1; i <= point.X + 1; i++)
            {
                for (var j = point.Y - 1; j <= point.Y + 1; j++)
                {
                    try
                    {
                        if (board[i, j] == ECellState.Ship)
                        {
                            return true;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }
            }

            return false;
        }
    }
    public class BoatLocationValidatorResponse : BaseValidatorResponse
    {
    }
}
