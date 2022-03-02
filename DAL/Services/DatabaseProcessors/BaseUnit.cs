using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.DTO;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.DatabaseProcessors
{
    public class BaseUnit
    {
        private readonly JsonSerializerOptions? _jsonOptions = new()
        {
            WriteIndented = true
        };

        protected readonly AppDbContext Context;

        protected BaseUnit(AppDbContext context)
        {
            Context = context;
        }
        
        protected string GetSerializedGameState(GameEngineDto gameEngineDto)
        {
            return JsonSerializer.Serialize(gameEngineDto, _jsonOptions);
        }

        protected async Task<Session>? GetSessionById(int sessionId)
        {
            return await Context.Sessions.FirstOrDefaultAsync(s => s.Id.Equals(sessionId));
        }
        
        protected void Save()
        {
            Context.SaveChanges();
        }
    }
}
