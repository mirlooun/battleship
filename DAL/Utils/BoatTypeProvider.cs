using System.Collections.Generic;
using DAL.DAL.Entities.Enums;

namespace DAL.Utils
{
    public static class BoatTypeProvider
    {
        private static readonly Dictionary<EBoatType, (string Name, int Size)> BoatTypes =
            new()
            {
                { EBoatType.Carrier, ("Carrier", 5) },
                { EBoatType.Battleship, ("Battleship", 4) },
                { EBoatType.Cruiser, ("Cruiser", 3) },
                { EBoatType.Submarine, ("Submarine", 3) },
                { EBoatType.Patrol, ("Patrol", 2) }
            };

        public static string GetUiName(EBoatType type)
        {
            return BoatTypes[type].Name;
        }

        public static int GetLength(EBoatType type)
        {
            return BoatTypes[type].Size;
        }
    }
}