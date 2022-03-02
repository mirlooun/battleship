using System.Collections.Generic;
using System.Linq;
using DAL.DAL.Entities;

namespace DAL.Helpers
{
    public static class TargetBoatFinder
    {
        public static BoatDal? FindBoat(IEnumerable<BoatDal> boats, LocationDal hit)
        {
            var boat = boats.FirstOrDefault(
                boat => boat.Locations
                    .FirstOrDefault(pl => pl.X.Equals(hit.X) && pl.Y.Equals(hit.Y)) != null
            );
            return boat;
        }
    }
}