using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.DTO;
using DAL.Mappers;
using DAL.Services;
using DAL.Services.DatabaseProcessors;
using GameSetupUiProviders.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class CreateNewPlayer : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }
        [BindProperty] public string FirstPlayerName { get; set; } = "";
        [BindProperty] public string SecondPlayerName { get; set; } = "";
        [BindProperty] public List<string> Messages { get; set; } = new ();
        public CreateNewPlayer(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var response1 = PlayerValidator.IsNameValid(FirstPlayerName);
            var response2 = PlayerValidator.IsNameValid(SecondPlayerName);

            if (!response1.IsValid || !response2.IsValid)
            {
                if (!response1.IsValid)
                {
                    Messages.Add("First player name: " + response1.Message);
                }

                if (!response2.IsValid)
                {
                    Messages.Add("Second player name: " + response2.Message);
                } 
                
                return Page();
            }
            
            var gameStateDtoFromDatabase = await DatabaseGameSessionProcessorUnit
                .GetLoader(_context)
                .LoadGameStateDtoFromDatabase(SessionId);
            
            var p1 = new PlayerDto
            {
                Name = FirstPlayerName,
                Boats = new List<BoatDto>(),
                MadeHits = new List<LocationPointDto>()
            };

            var p2 = new PlayerDto
            {
                Name = SecondPlayerName,
                Boats = new List<BoatDto>(),
                MadeHits = new List<LocationPointDto>()
            };
            
            gameStateDtoFromDatabase.Players.Add(p1);
            gameStateDtoFromDatabase.Players.Add(p2);
            
            DatabaseGameSessionProcessorUnit
                .GetUpdater(_context)
                .UpdateGameStateInDatabase(GameEngineMapper.MapToDal(gameStateDtoFromDatabase));

            return RedirectToPage("./PlaceBoats",
                new {  SessionId, p1Completed = false, p2Completed = false });
        }
    }
}