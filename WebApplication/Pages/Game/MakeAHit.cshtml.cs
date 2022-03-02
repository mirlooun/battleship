using System;
using System.Text.Json;
using DAL;
using DAL.DAL.Entities;
using DAL.DTO;
using DAL.Helpers;
using DAL.Mappers;
using DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class MakeAHit : PageModel
    {
        private readonly AppDbContext _context;


        [BindProperty(Name = "SessionId", SupportsGet = true)]
        public int SessionId { get; set; }


        [BindProperty(Name = "JsonGameEngine", SupportsGet = true)]
        public string JsonGameEngine { get; set; } = default!;

        public GameEngine GameEngine { get; set; } = default!;
        public GameSettingsDal GameSettings => GameEngine.GetGameSettings();


        [BindProperty(Name = "JsonHit", SupportsGet = true)]
        public string JsonHit { get; set; } = default!;
        public LocationPointDto Hit { get; set; } = default!;


        [BindProperty(Name = "PreviousMoveDirection", SupportsGet = true)]
        public string PreviousMoveDirection { get; set; } = "up";

        [BindProperty] public string Direction { get; set; } = "up";


        [BindProperty(Name = "JsonHitResponse", SupportsGet = true)]
        public string? JsonHitResponse { get; set; }
        private HitResponse? HitResponse { get; set; }

        public MakeAHit(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            LoadState();

            return Page();
        }

        public IActionResult OnPostChange()
        {
            LoadState();

            var hitAttempt = new LocationDal(Hit.X, Hit.Y);

            switch (Direction)
            {
                case "up":
                    HitLocationChanger.TryDeltaMove(ref hitAttempt, 0, -1, GameSettings);
                    break;
                case "down":
                    HitLocationChanger.TryDeltaMove(ref hitAttempt, 0, 1, GameSettings);
                    break;
                case "left":
                    HitLocationChanger.TryDeltaMove(ref hitAttempt, -1, 0, GameSettings);
                    break;
                case "right":
                    HitLocationChanger.TryDeltaMove(ref hitAttempt, 1, 0, GameSettings);
                    break;
            }

            UpdateHitWithNewValue(hitAttempt);

            return RedirectToPage("./MakeAHit",
                new { SessionId, JsonHit, PreviousMoveDirection = Direction, JsonGameEngine });
        }

        private void UpdateHitWithNewValue(LocationDal hitAttempt)
        {
            Hit.X = hitAttempt.X;
            Hit.Y = hitAttempt.Y;
            JsonHit = JsonSerializer.Serialize(Hit);
        }

        public IActionResult OnPostHit()
        {
            LoadState();

            HitResponse = GameEngine.MakeAHit(new LocationDal(Hit.X, Hit.Y));
            
            DatabaseController.UpdateGameStateInDatabase(GameEngine, _context);
            
            var dto = HitResponse.GetDto();
            JsonHitResponse = JsonSerializer.Serialize(dto);
            
            return RedirectToPage("./DisplayMessage",
                new { SessionId, JsonHitResponse, JsonHit, PreviousMoveDirection });
        }

        private  void LoadState()
        {
            // Load first time then pass as json
            if (string.IsNullOrEmpty(JsonGameEngine))
            {
                GameEngine = DatabaseController.LoadGameFromDatabase(SessionId, _context);
                JsonGameEngine = JsonSerializer.Serialize(GameEngine.GetGameStateDto());
            }
            else
            {
                var gameEngineDto = JsonSerializer.Deserialize<GameEngineDto>(JsonGameEngine)!;
                GameEngine = GameEngineMapper.MapToDal(gameEngineDto);
                JsonGameEngine = JsonSerializer.Serialize(GameEngine.GetGameStateDto());
            }

            if (string.IsNullOrEmpty(JsonHit))
            {
                Hit = new LocationPointDto()
                {
                    X = 0,
                    Y = 0
                };
                JsonHit = JsonSerializer.Serialize(Hit);
            }
            else
            {
                Hit = JsonSerializer.Deserialize<LocationPointDto>(JsonHit)!;
                JsonHit = JsonSerializer.Serialize(Hit);
            }

            if (!string.IsNullOrEmpty(JsonHitResponse))
            {
                var responseDto = JsonSerializer.Deserialize<HitResponseDto>(JsonHitResponse)!;
                HitResponse = new HitResponse(
                    responseDto.IsHit,
                    responseDto.BoatName,
                    responseDto.IsDestroyed,
                    responseDto.IsSameCell
                );
            }
        }
    }
}