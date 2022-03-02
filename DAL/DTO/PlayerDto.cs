using System.Collections.Generic;

namespace DAL.DTO
{
    public class PlayerDto
    {
        public string Name { get; set; } = default!;
        public List<BoatDto> Boats { get; set; } = default!;
        public List<LocationPointDto> MadeHits { get; set; } = default!;
    }
}
