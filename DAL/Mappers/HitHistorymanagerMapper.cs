using System.Collections.Generic;
using DAL.DTO;
using DAL.Services;

namespace DAL.Mappers
{
    public static class HitHistoryManagerMapper
    {
        public static HitHistoryManagerDto MapToDto(HitHistoryManager historyManager)
        {
            return new HitHistoryManagerDto
            {
                HitHistory = new List<HitRecord>(historyManager.GetHistory())
            };
        }
        
        public static HitHistoryManager MapToDal(HitHistoryManagerDto historyManagerDto)
        {
            historyManagerDto.HitHistory.Reverse();
            return new HitHistoryManager(historyManagerDto.HitHistory);
        }
    }
}