using System.Collections.Generic;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums.Rules;

namespace DAL.DTO
{
    public class GameSettingsDto
    {
        public int FieldHeight { get; set; }
        public int FieldWidth { get; set; }
        public EHitContinuousMove HitContinuousMove { get; set; }
        public EBoatsCanTouch BoatsCanTouch { get; set; }
        public List<BoatConfigurationDto> BoatsConfig { get; set; } = default!;
    }
}
