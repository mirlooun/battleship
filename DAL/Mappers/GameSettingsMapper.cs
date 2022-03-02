using DAL.DAL.Entities;
using DAL.DTO;
using Domain;

namespace DAL.Mappers
{
    public static class GameSettingsMapper
    {
        public static GameSettingsDto MapToDto(GameSettingsDal gameSettings)
        {
            return new GameSettingsDto
            {
                FieldHeight = gameSettings.FieldHeight,
                FieldWidth = gameSettings.FieldWidth,
                HitContinuousMove = gameSettings.HitContinuousMove,
                BoatsCanTouch = gameSettings.BoatsCanTouch,
                BoatsConfig = gameSettings.BoatsConfig
            };
        }
        
        public static GameSettingsDal MapToDal(GameSettingsDto gameSettings)
        {
            return new GameSettingsDal
            {
                FieldHeight = gameSettings.FieldHeight,
                FieldWidth = gameSettings.FieldWidth,
                HitContinuousMove = gameSettings.HitContinuousMove,
                BoatsCanTouch = gameSettings.BoatsCanTouch,
                BoatsConfig = gameSettings.BoatsConfig
            };
        }
    }
}