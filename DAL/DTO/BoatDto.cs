using System.Collections.Generic;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;

namespace DAL.DTO
{
    public class BoatDto
    {
        public EBoatType Type { get; set; }
        public List<LocationPointDto>? Locations { get; set; }
    }
    
    public class BoatDtoFull
    {
        public EBoatType Type { get; set; }
        public EBoatDirection BoatDirection { get; set; }
        public LocationPointDto? StartsAt { get; set; }
    }
}
