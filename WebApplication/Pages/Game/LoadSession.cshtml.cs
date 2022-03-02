using System.Collections.Generic;
using DAL;
using DAL.Services;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class LoadSession : PageModel
    {
        private readonly AppDbContext _context;
        public List<Session> Sessions { get; set; } = default!;

        public LoadSession(AppDbContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            Sessions = DatabaseController.GetGameSessions(_context).Result;
        }
    }
}