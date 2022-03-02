using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DAL;
using DAL.DTO;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class DisplayMessage : PageModel
    {
        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }


        [BindProperty(Name = "JsonHitResponse", SupportsGet = true)]
        public string? JsonHitResponse { get; set; }
        private HitResponse? HitResponse { get; set; }
        
        
        [BindProperty(Name = "JsonHit", SupportsGet = true)]
        public string? JsonHit { get; set; }
        
        
        [BindProperty(Name = "PreviousMoveDirection", SupportsGet = true)]
        public string? PreviousMoveDirection { get; set; }
        
        [BindProperty(Name = "GameIsFinished", SupportsGet = true)]
        
        public bool? GameIsFinished { get; set; }

        private GameEngine _gameEngine = default!;

        public LocationPointDto Hit { get; set; } = default!;

        [BindProperty] public List<string> Messages { get; set; } = new();

        private readonly AppDbContext _context;

        public DisplayMessage(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (GameIsFinished != null)
            {
                _gameEngine = DatabaseController.LoadGameFromDatabase(SessionId, _context);
                var (winner, loser) = _gameEngine.GetWinnerAndLoser();
                var message = $"Session winner is {winner} and loser is {loser}";
                Messages.Add(message); 
            }
            else
            {
                LoadState();
                var message = HitResponse!.GetMessage();
                Messages.Add(message);  
            }

            
            return Page();
        }

        private void LoadState()
        {
            Hit = JsonSerializer.Deserialize<LocationPointDto>(JsonHit!)!;
            JsonHit = JsonSerializer.Serialize(Hit);
            
            var responseDto = JsonSerializer.Deserialize<HitResponseDto>(JsonHitResponse!)!;
            HitResponse = new HitResponse(
                responseDto.IsHit, 
                responseDto.BoatName, 
                responseDto.IsDestroyed,
                responseDto.IsSameCell);
        }

        public IActionResult OnPostContinue()
        {
            LoadState();
            
           _gameEngine = DatabaseController.LoadGameFromDatabase(SessionId, _context);
            
            if (!HitResponse!.IsSamePlayerMove(_gameEngine.GetGameSettings().HitContinuousMove))
            {
                return RedirectToPage("./PlayBattleship", new { SessionId });
            }

            return RedirectToPage("./MakeAHit", new { SessionId, JsonHit, PreviousMoveDirection  });
        }
    }
}