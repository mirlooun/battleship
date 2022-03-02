using System.Linq;
using DAL.DAL.Entities;
using DAL.DTO;

namespace DAL.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerDto MapToDto(PlayerDal player)
        {
            var dto = new PlayerDto
            {
                Name = player.Name,
                MadeHits = player.GetHits().Select(LocationPointMapper.MapToDto).ToList(),
                Boats = player.GetBoats().Select(BoatMapper.MapToDto).ToList()
            };
            return dto;
        }
        
        public static PlayerDal MapToDal(PlayerDto player)
        {
            return new PlayerDal(
                player.Name,
                player.Boats.Select(BoatMapper.MapToDal).ToList(),
                player.MadeHits.Select(LocationPointMapper.MapToDal).ToList()
                );
        }
    }
}
