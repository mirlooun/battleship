using DAL.DAL.Entities;
using DAL.DTO;
using Domain;

namespace DAL.Mappers
{
    public static class LocationPointMapper
    {
        public static LocationPointDto MapToDto(LocationPointDal locationPoint)
        {
            return new LocationPointDto
            {
                X = locationPoint.X,
                Y = locationPoint.Y,
                PointState = locationPoint.PointState
            };
        }
        
        public static LocationPointDal MapToDal(LocationPointDto locationPoint)
        {
            return new LocationPointDal(locationPoint.X, locationPoint.Y, locationPoint.PointState);
        }
    }
}