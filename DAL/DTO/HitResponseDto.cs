namespace DAL.DTO
{
    public class HitResponseDto
    {
        public bool IsHit { get; set;  }
        public string? BoatName { get; set; }
        public bool IsDestroyed { get; set; }
        public bool IsSameCell { get; set; }
    }
}