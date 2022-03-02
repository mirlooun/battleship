using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.DTO;
using DAL.Services.DatabaseProcessors;
using DAL.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication.Pages.Game
{
    public class PlaceBoats : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty(Name = "Player1Completed", SupportsGet = true)]
        public bool Player1Completed { get; set; }

        [BindProperty(Name = "Player2Completed", SupportsGet = true)]
        private bool Player2Completed { get; set; }

        public ECellState[,] PlayerCurrentBoardState { get; set; } = default!;
        public Dictionary<EBoatType, int> RemainingBoatCount { get; set; } = new();
        [BindProperty] public EBoatType BoatType { get; set; }

        public (string p1, string p2)? PlayerNames { get; set; }

        [BindProperty] public List<string> Messages { get; set; } = new();
        public PlaceBoats(AppDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            LoadState();
        }

        private void LoadState()
        {
            var gameStateDtoFromDatabase = DatabaseGameSessionProcessorUnit
                .GetLoader(_context)
                .LoadGameStateDtoFromDatabase(SessionId).Result;

            PlayerNames = gameStateDtoFromDatabase.GetPlayerNamesInOrder();

            var currentPlayer = !Player1Completed
                ? gameStateDtoFromDatabase.GetFirstPlayer()
                : gameStateDtoFromDatabase.GetSecondPlayer();

            var playerBoatTypes = currentPlayer.Boats.Select(c => c.Type).ToList();

            var boatsConfig = gameStateDtoFromDatabase.GameSettings.BoatsConfig;

            foreach (var config in boatsConfig
                .Where(config => playerBoatTypes.Contains(config.BoatType)))
            {
                config.BoatCount -= 1;
            }

            if (boatsConfig.Count > 0)
            {
                boatsConfig.ForEach(e => RemainingBoatCount.Add(e.BoatType, e.BoatCount));
            }

            PlayerCurrentBoardState = PlayerBoardProvider
                .GetPlayerBoardWithBoats(
                    currentPlayer.Boats,
                    gameStateDtoFromDatabase.GameSettings
                );
        }

        public IActionResult OnPost()
        {
            LoadState();

            RemainingBoatCount.TryGetValue(BoatType, out var boatCount);
            
            if (boatCount == 0)
            {
                Messages.Add($"All boats of type {BoatTypeProvider.GetUiName(BoatType)} are placed");
                return Page();
            }

            var boat = new BoatDtoFull()
            {
                Type = BoatType,
                BoatDirection = EBoatDirection.Horizontal,
                StartsAt = new LocationPointDto()
            };

            var boatString = JsonSerializer.Serialize(boat);

            return RedirectToPage("./PlaceSingleBoat",
                new
                {
                    SessionId,
                    Player1Completed,
                    Player2Completed,
                    Boat = boatString
                });
        }
    }
}