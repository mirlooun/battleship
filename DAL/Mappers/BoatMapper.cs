using System.Linq;
using DAL.DAL.Entities;
using DAL.DTO;

namespace DAL.Mappers
{
    public static class BoatMapper
    {
        public static BoatDto MapToDto(BoatDal boat)
        {
            return new BoatDto
            {
                Type = boat.Type,
                Locations = boat.Locations.Select(LocationPointMapper.MapToDto).ToList()
            };
        }

        public static BoatDal MapToDal(BoatDto boat)
        {
            var locations = boat.Locations!.Select(LocationPointMapper.MapToDal).ToList();
            return new BoatDal(locations, boat.Type, true);
        }
    }
}