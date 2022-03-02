using System;
using DAL.DAL.Entities.Enums;

namespace DAL.DAL.Entities
{
    public class LocationPointDal : LocationDal
    {
        public LocationPointDal(int x, int y, ECellState pointState) : base(x, y)
        {
            PointState = pointState;
        }
        public ECellState PointState { get; set; }
        public bool Equals(LocationDal location)
        {
            return X.Equals(location.X) && Y.Equals(location.Y);
        }
    }
}
