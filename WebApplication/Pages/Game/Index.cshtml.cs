using System;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.DTO;
using DAL.Services;
using DAL.Services.DatabaseProcessors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class Index : PageModel
    {
        private readonly AppDbContext _context;
        
        [BindProperty] public GameSettingsDto GameSettings { get; set; }

        public Index(AppDbContext context)
        {
            _context = context;
            GameSettings = GameSettingsController.GetDefaultGameSettingsDto();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCreate()
        {
            var session = DatabaseGameSessionProcessorUnit
                .GetCreator(_context)
                .CreateSession(GameSettings);
            
            return RedirectToPage("./CreateNewPlayer", new { SessionId = session.Id });
        }

        public IActionResult OnPostLoad()
        {
            return RedirectToPage("./LoadSession");
        }
    }
}