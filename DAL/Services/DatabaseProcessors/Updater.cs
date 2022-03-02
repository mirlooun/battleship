using System;
using System.Text.Json;
using DAL.DTO;

namespace DAL.Services.DatabaseProcessors
{
    public class Updater : BaseUnit
    {
        public Updater(AppDbContext context) : base(context)
        {
        }

        public void UpdateGameStateInDatabase(GameEngine gameEngine)
        {
            var gameEngineDto = gameEngine.GetGameStateDto();
            
            var session = GetSessionById(gameEngineDto.SessionId!.Value)!.Result;

            var jsonGameState = GetSerializedGameState(gameEngineDto);

            session.LastUpdate = DateTime.Now;

            session.JsonState = jsonGameState;

            Save();
        }
    }
}
