using System.Collections.Generic;
using DAL.Services;

namespace DAL.DTO
{
    public class HitHistoryManagerDto
    {
        public List<HitRecord> HitHistory { get; set; } = default!;
    }
}