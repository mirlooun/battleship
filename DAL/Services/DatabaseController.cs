using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Services.DatabaseProcessors;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    
    public static class DatabaseController
    {
        #region Save to db
        public static void SaveGameToDatabase(GameEngine gameEngine)
        {
            using var context = new AppDbContext();
            DatabaseGameSessionProcessorUnit.GetCreator(context).AddGameStateToDatabase(gameEngine);
        }
        
        [Obsolete("Not important implementation")]
        public static void SaveGameToDatabase(GameEngine gameEngine, AppDbContext context)
        {
            DatabaseGameSessionProcessorUnit.GetCreator(context).AddGameStateToDatabase(gameEngine);
        }
        
        #endregion

        #region Update gamestate in db

        public static void UpdateGameStateInDatabase(GameEngine gameEngine)
        {
            using var context = new AppDbContext();
            DatabaseGameSessionProcessorUnit
                .GetUpdater(context)
                .UpdateGameStateInDatabase(gameEngine);
        }
        
        public static void UpdateGameStateInDatabase(GameEngine gameEngine, AppDbContext context)
        {
            DatabaseGameSessionProcessorUnit.GetUpdater(context).UpdateGameStateInDatabase(gameEngine);
        }
        
        #endregion

        #region Load from db
        public static async Task<GameEngine> LoadGameFromDatabase(int sessionId)
        {
            await using var context = new AppDbContext();
            return DatabaseGameSessionProcessorUnit.GetLoader(context).LoadGameStateFromDatabase(sessionId);
        }
        
        public static GameEngine LoadGameFromDatabase(int sessionId, AppDbContext context)
        {
            return DatabaseGameSessionProcessorUnit.GetLoader(context).LoadGameStateFromDatabase(sessionId);
        }

        #endregion
        public static List<Session> GetGameSessions()
        {
            using var db = new AppDbContext();
            return GetGameSessions(db).Result;
        }
        public static async Task<List<Session>> GetGameSessions(AppDbContext db)
        {
            var sessions = await db.Sessions
                .Where(s => !s.Name.Equals("undefined"))
                .OrderByDescending(s => s.LastUpdate)
                .ToListAsync();
            return sessions;
        }
    }
}
