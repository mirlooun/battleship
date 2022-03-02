using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTO;
using Domain;

namespace DAL.Services.DatabaseProcessors
{
    public class Creator : BaseUnit
    {

        public Creator(AppDbContext context) : base(context)
        {
        }

        public void AddGameStateToDatabase(GameEngine gameEngine)
        {
            var gameEngineDto = gameEngine.GetGameStateDto();
            
            CreateSession(gameEngineDto);
            
            Save();
        }

        public Session CreateSession(GameSettingsDto gameSettingsDto)
        {
            gameSettingsDto.BoatsConfig = GameSettingsController.GetDefaultBoatConfiguration();
            
            var addedSession = Context.Sessions.Add(new Session
            {
                Name = "undefined",
                SessionStart = DateTime.Now,
                LastUpdate = DateTime.Now,
                JsonState = GetSerializedGameState(new GameEngineDto()
                {
                    NextMoveByFirst = true, 
                    GameSettings = gameSettingsDto,
                     HitHistoryManager = new HitHistoryManagerDto()
                     {
                         HitHistory = new List<HitRecord>()
                     }
                })
            });
            
            Save();

            return addedSession.Entity;
        }

        private void CreateSession(GameEngineDto gameEngineDto)
        {
            var (p1, p2) = gameEngineDto.GetPlayerNamesInOrder();

            var gameSaveName = $"{p1}_VS_{p2}";
            
            var jsonState = GetSerializedGameState(gameEngineDto);
            
            Context.Sessions.Add(new Session
            {
                Name = gameSaveName,
                SessionStart = DateTime.Now,
                LastUpdate = DateTime.Now,
                JsonState = jsonState,
            });
        }
    }
}
