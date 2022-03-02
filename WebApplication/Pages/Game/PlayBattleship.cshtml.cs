using System;
using System.Threading.Tasks;
using DAL;
using DAL.DAL.Entities;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class PlayBattleship : PageModel
    {
        private readonly AppDbContext _context;
        
        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }
        public GameEngine? GameEngine { get; set; }
        public GameSettingsDal? GameSettings { get; set; }

        public PlayBattleship(AppDbContext context)
        {
            _context = context;
            
        }

        public IActionResult OnGet()
        {
            GameEngine = DatabaseController.LoadGameFromDatabase(SessionId, _context);
            
            if (GameEngine.HasWinner())
            {
                return RedirectToPage("./DisplayMessage", new
                {
                    SessionId,
                    GameIsFinished = true
                });
            }
            GameSettings = GameEngine.GetGameSettings();
            
            return Page();
        }

        public IActionResult OnPostHit()
        {
            return RedirectToPage("./MakeAHit", new
            {
                SessionId
            });
        }
        
        public IActionResult OnPostRevert()
        {
            return RedirectToPage("./RevertAHit", new
            {
                SessionId
            });
        }

        public IActionResult OnPostBack()
        {
            return RedirectToPage("./Index");
        }
    }
}