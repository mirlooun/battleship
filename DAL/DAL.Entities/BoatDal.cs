using System.Collections.Generic;
using System.Linq;
using DAL.DAL.Entities.Enums;
using DAL.Utils;

namespace DAL.DAL.Entities
{
    public class BoatDal
    {
        public BoatDal(List<LocationPointDal> locations, EBoatType type, bool isPlaced)
        {
            _locations = locations;
            Type = type;
            IsPlaced = isPlaced;
        }

        public EBoatType Type { get; }

        private List<LocationPointDal>? _locations;
        public List<LocationPointDal> Locations
        {
            get
            {
                _locations ??= GenerateLocations(StartsAt!, Type, Direction);
                return IsPlaced ? _locations : GenerateLocations(StartsAt!, Type, Direction);
            }
        }
        private bool IsPlaced { get; set; }
        public void SetPlaced()
        {
            IsPlaced = true;
        }
        public LocationDal? StartsAt { get; set; }
        public EBoatDirection Direction { get; set; }
        public BoatDal(EBoatType type)
        {
            Type = type;
            StartsAt = new LocationDal(0, 0);
            Direction = EBoatDirection.Horizontal;
        }

        public BoatDal(BoatDal from)
        {
            Type = from.Type;
            StartsAt = new LocationDal(from.StartsAt!.X, from.StartsAt!.Y);
            Direction = from.Direction;
        }
        private static List<LocationPointDal> GenerateLocations(LocationDal start, EBoatType type, EBoatDirection direction)
        {
            var locations = new List<LocationPointDal>();
            
            for (var i = 0; i < BoatTypeProvider.GetLength(type); i++)
            {
                var location = new LocationPointDal(
                    start.X + (direction == EBoatDirection.Horizontal ? i : 0),
                    start.Y + (direction == EBoatDirection.Vertical ? i : 0),
                    ECellState.Ship
                );

                locations.Add(location);
            }

            return locations;
        }
        public string GetName()
        {
            return BoatTypeProvider.GetUiName(Type);
        }
        public int GetHp()
        {
            return _locations!.Count(l => l.PointState == ECellState.Ship);
        }

        public void MakeAHit(LocationDal hit)
        {
            var point = Locations.Find(pl =>
                pl.X.Equals(hit.X) &&
                pl.Y.Equals(hit.Y));
            point!.PointState = ECellState.Hit;
        }

        public bool IsLocatedHere(int colIndex, int rowIndex)
        {
            return Locations.Exists(point => point.X == colIndex && point.Y == rowIndex);
        }
    }
    
    public enum EBoatDirection
    {
        Vertical,
        Horizontal
    }
}
