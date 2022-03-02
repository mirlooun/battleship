using System.Text.Json;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Mappers;

namespace DAL.Services.DatabaseProcessors
{
    public class Loader : BaseUnit
    {
        public Loader(AppDbContext context) : base(context)
        {
        }

        public GameEngine LoadGameStateFromDatabase(int sessionId)
        {
            var session = GetSessionById(sessionId)!.Result;

            var deserializedGameState = JsonSerializer.Deserialize<GameEngineDto>(session.JsonState)!;

            deserializedGameState.SessionId = sessionId;
            
            var gameEngine = GameEngineMapper.MapToDal(deserializedGameState);

            return gameEngine;
        }
        public async Task<GameEngineDto> LoadGameStateDtoFromDatabase(int sessionId)
        {
            var session = await GetSessionById(sessionId)!;

            var deserializedGameState = JsonSerializer.Deserialize<GameEngineDto>(session.JsonState);

            deserializedGameState!.SessionId = sessionId;
            
            return deserializedGameState;
        }
    }
}
