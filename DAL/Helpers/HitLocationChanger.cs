using DAL.DAL.Entities;

namespace DAL.Helpers
{
    public static class HitLocationChanger
    {
        public static void TryDeltaMove(ref LocationDal hitLocation, int deltaX, int deltaY, GameSettingsDal gs)
        {
            var attempt = new LocationDal(hitLocation);
            attempt.X += deltaX;
            attempt.Y += deltaY;
            
            if (IsValidPosition(attempt, gs))
            {
                hitLocation = attempt;
            }
        }

        private static bool IsValidPosition(LocationDal hitLocation, GameSettingsDal gameSettings)
        {
            return hitLocation.X >= 0 &&
                   hitLocation.Y >= 0 &&
                   hitLocation.X < gameSettings.FieldWidth &&
                   hitLocation.Y < gameSettings.FieldHeight;
        }
    }
}
