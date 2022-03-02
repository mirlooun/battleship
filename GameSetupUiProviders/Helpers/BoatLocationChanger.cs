using System;
using System.Linq;
using DAL.DAL.Entities;

namespace GameSetupUiProviders.Helpers
{
    public static class BoatLocationChanger
    {
        public static void TryDeltaMove(ref BoatDal boat, int deltaX, int deltaY, GameSettingsDal gs)
        {
            var attempt = new BoatDal(boat);
            attempt.StartsAt!.X += deltaX;
            attempt.StartsAt.Y += deltaY;

            if (IsValidPosition(attempt, gs))
            {
                boat = attempt;
            }
        }
        
        public static void TryRotate(ref BoatDal boat, GameSettingsDal gs)
        {
            var oldLocations = boat.Locations;
            var offset = oldLocations.Count / 2;

            var newStartsAt = oldLocations[offset];
            if (boat.Direction == EBoatDirection.Horizontal)
            {
                newStartsAt.Y -= offset;
            }
            else
            {
                newStartsAt.X -= offset;
            }

            var attempt = new BoatDal(boat);
            attempt.StartsAt = newStartsAt;
            attempt.Direction = attempt.Direction switch
            {
                EBoatDirection.Horizontal => EBoatDirection.Vertical,
                EBoatDirection.Vertical => EBoatDirection.Horizontal,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (IsValidPosition(attempt, gs))
            {
                boat = attempt;
            }
        }

        private static bool IsValidPosition(BoatDal boat, GameSettingsDal gameSettings)
        {
            return boat.Locations.All(pos => pos.X >= 0 && 
                                             pos.Y >= 0 && 
                                             pos.X < gameSettings.FieldWidth && 
                                             pos.Y < gameSettings.FieldHeight
            );
        }
    }
}
