using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DAL.Entities.Enums;
using DAL.DAL.Entities.Enums.Rules;

namespace DAL.DAL.Entities
{
    public class GameSettingsDal
    {
        public int FieldHeight { get; set; }
        public int FieldWidth { get; set; }
        public EBoatsCanTouch BoatsCanTouch { get; set; }
        public EHitContinuousMove HitContinuousMove { get; set; }
        public ESavingMethod SavingMethod { get; set; }
        public List<BoatConfigurationDto> BoatsConfig { get; set; } = default!;
        public Dictionary<EBoatType, int> GetBoatsConfiguration()
        {
            return BoatsConfig.ToDictionary(entry => entry.BoatType, entry => entry.BoatCount);
        }
        
        // TODO: implement theme color choosing
        public ConsoleColor ThemeColor { get; set; }
    }
    
    public class BoatConfigurationDto
    {
        public EBoatType BoatType { get; init; }
        public int BoatCount { get; set; }
    }
}
