using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.DTO;
using DAL.Mappers;
using DAL.Services;
using DAL.Services.DatabaseProcessors;
using DAL.Utils;
using GameSetupUiProviders.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication.Pages.Game
{
    public class PlaceSingleBoat : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty(Name = "Player1Completed", SupportsGet = true)]
        public bool Player1Completed { get; set; }
        
        [BindProperty(Name = "Player2Completed", SupportsGet = true)]
        public bool Player2Completed { get; set; }

        [BindProperty(Name = "Boat", SupportsGet = true)]
        public string Boat { get; set; } = default!;

        [BindProperty(Name = "PreviousMoveDirection", SupportsGet = true)]
        public string PreviousMoveDirection { get; set; } = "up";

        public PlaceSingleBoat(AppDbContext context)
        {
            _context = context;
        }
        public BoatDal? PreliminaryBoat { get; set; }

        public ECellState[,]? PlayerCurrentBoardState { get; set; }

        public void OnGet()
        {
            var gameStateDtoFromDatabase = DatabaseGameSessionProcessorUnit
                .GetLoader(_context)
                .LoadGameStateDtoFromDatabase(SessionId)
                .Result;

            var boatDto = JsonSerializer.Deserialize<BoatDtoFull>(Boat);

            PreliminaryBoat = new BoatDal(boatDto!.Type)
            {
                Direction = boatDto.BoatDirection,
                StartsAt = new LocationDal(boatDto.StartsAt!.X, boatDto.StartsAt.Y)
            };

            PlayerDto currentPlayer = !Player1Completed
                ? gameStateDtoFromDatabase.GetFirstPlayer()
                : gameStateDtoFromDatabase.GetSecondPlayer();
            
            PlayerCurrentBoardState = PlayerBoardProvider
                .GetPlayerBoardWithBoats(
                    currentPlayer.Boats,
                    gameStateDtoFromDatabase.GameSettings
                );
        }
        
        [BindProperty(Name = "Direction", SupportsGet = true)]
        public string Direction { get; set; } = "";
        
        public IActionResult OnPostMove()
        {
            var gameStateDtoFromDatabase = DatabaseController.LoadGameFromDatabase(SessionId, _context);

            var gameSettings = gameStateDtoFromDatabase.GetGameSettings();
            
            var dtoBoat = JsonSerializer.Deserialize<BoatDtoFull>(Boat);

            var boat = new BoatDal(dtoBoat!.Type)
            {
                Direction = dtoBoat.BoatDirection,
                StartsAt = new LocationDal(dtoBoat.StartsAt!.X, dtoBoat.StartsAt!.Y)
            };
            
            switch (Direction)
            {
                case "up":
                    BoatLocationChanger.TryDeltaMove(ref boat, 0, -1, gameSettings);
                    break;
                case "down":
                    BoatLocationChanger.TryDeltaMove(ref boat, 0, 1, gameSettings);
                    break;
                case "left":
                    BoatLocationChanger.TryDeltaMove(ref boat, -1, 0, gameSettings);
                    break;
                case "right":
                    BoatLocationChanger.TryDeltaMove(ref boat, 1, 0, gameSettings);
                    break;
                case "rotate":
                    BoatLocationChanger.TryRotate(ref boat, gameSettings);
                    break;
            }

            var jsonBoat = JsonSerializer.Serialize(new BoatDtoFull()
            {
                Type = boat.Type,
                BoatDirection = boat.Direction,
                StartsAt = new LocationPointDto()
                {
                    X = boat.StartsAt!.X,
                    Y = boat.StartsAt!.Y
                }
            });

            return RedirectToPage("./PlaceSingleBoat",
                new
                {
                    SessionId,
                    Player1Completed,
                    Player2Completed,
                    Boat=jsonBoat,
                    PreviousMoveDirection=Direction
                });
        }
        
        [BindProperty] public List<string> Messages { get; set; } = new ();

        public async Task<IActionResult> OnPostSubmit()
        {
            var gameStateDtoFromDatabase = await DatabaseGameSessionProcessorUnit
                .GetLoader(_context)
                .LoadGameStateDtoFromDatabase(SessionId);

            var currentPlayer = !Player1Completed
                ? gameStateDtoFromDatabase.GetFirstPlayer()
                : gameStateDtoFromDatabase.GetSecondPlayer();

            var board = PlayerBoardProvider.GetPlayerBoardWithBoats(currentPlayer.Boats, gameStateDtoFromDatabase.GameSettings);

            var rule = gameStateDtoFromDatabase.GameSettings.BoatsCanTouch;
            
            var boatDto = JsonSerializer.Deserialize<BoatDtoFull>(Boat);

            var boatToPlace = new BoatDal(boatDto!.Type)
            {
                Direction = boatDto.BoatDirection,
                StartsAt = new LocationDal(boatDto.StartsAt!.X, boatDto.StartsAt!.Y)
            };
            
            PlayerCurrentBoardState = PlayerBoardProvider
                .GetPlayerBoardWithBoats(
                    currentPlayer.Boats,
                    gameStateDtoFromDatabase.GameSettings
                );
            
            PreliminaryBoat = new BoatDal(boatDto.Type)
            {
                Direction = boatDto.BoatDirection,
                StartsAt = new LocationDal(boatDto.StartsAt!.X, boatDto.StartsAt.Y)
            };

            var response = BoatLocationValidator.IsBoatLocationOccupied(board, boatToPlace, rule);
            
            /* Validation */
            
            if (!response.IsValid)
            {
                Messages.Add(response.Message);

                return Page();
            }
            
            currentPlayer.Boats.Add(BoatMapper.MapToDto(boatToPlace));

            DatabaseGameSessionProcessorUnit
                .GetUpdater(_context)
                .UpdateGameStateInDatabase(GameEngineMapper.MapToDal(gameStateDtoFromDatabase));

            Player1Completed = gameStateDtoFromDatabase.GetFirstPlayer().Boats.Count 
                               == gameStateDtoFromDatabase.GameSettings.BoatsConfig.Count;
            Player2Completed = gameStateDtoFromDatabase.GetSecondPlayer().Boats.Count 
                               == gameStateDtoFromDatabase.GameSettings.BoatsConfig.Count;

            if (Player1Completed && Player2Completed)
            {
                UpdateSessionName(gameStateDtoFromDatabase.GetPlayerNamesInOrder());
                
                return RedirectToPage("./PlayBattleship", new { SessionId });
            }

            return RedirectToPage("./PlaceBoats",
                new {  SessionId, Player1Completed, Player2Completed });
        }
        
        private void UpdateSessionName((string p1, string p2) getPlayerNamesInOrder)
        {
            var (p1, p2) = getPlayerNamesInOrder;
            var session = _context.Sessions.First(s => s.Id.Equals(SessionId));

            var timeNow = DateTime.Now;
            
            var sessionStart = $"{timeNow:dd-MM-yyyy_HH-mm-ss}";
            
            session.Name = $"p1={p1}_p2={p2}_save_{sessionStart}";
            session.SessionStart = timeNow;
            session.LastUpdate = timeNow;
            
            _context.SaveChanges();
        }
    }
}