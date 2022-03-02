using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class RevertAHit : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }
        
        [BindProperty(Name = "HitRecordIndex", SupportsGet = true)]
        
        public int? HitRecordIndex { get; set; }

        private GameEngine? _gameEngine;
        public List<HitRecord> HitHistory => _gameEngine!.GetHitHistory().ToList();
        
        public RevertAHit(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            LoadState();

            if (HitRecordIndex != null)
            {
                var hitRecord = HitHistory[HitRecordIndex.Value];
                
                _gameEngine!.RevertMovesToSpecified(hitRecord);
            
                DatabaseController.UpdateGameStateInDatabase(_gameEngine, _context);

                return RedirectToPage("./PlayBattleship", new
                {
                    SessionId
                });
            }

            return Page();
        }

        private void LoadState()
        {
            _gameEngine = DatabaseController.LoadGameFromDatabase(SessionId, _context);
        }
    }
}