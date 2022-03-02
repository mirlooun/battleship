using System;

namespace Domain
{
    public class Session : BaseEntity
    {
        public string Name { get; set; } = default!;
        public DateTime SessionStart { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool IsFinished { get; set; }
        public string JsonState { get; set; } = default!;
    }
}