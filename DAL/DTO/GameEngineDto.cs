using System;
using System.Collections.Generic;

namespace DAL.DTO
{
    public class GameEngineDto
    {
        public int? SessionId { get; set; }
        public string? SaveFilePath { get; set; }
        public bool IsFinished { get; set; }
        public GameSettingsDto GameSettings { get; set; } = default!;
        public HitHistoryManagerDto HitHistoryManager { get; set; } = default!;
        public bool NextMoveByFirst { get; set; }
        public List<PlayerDto> Players { get; set; } = new ();
        public PlayerDto GetFirstPlayer() => Players[0];
        public PlayerDto GetSecondPlayer() => Players[1];
        public (string p1, string p2) GetPlayerNamesInOrder()
        {
            return (GetFirstPlayer().Name, GetSecondPlayer().Name);
        }
        
    }
}