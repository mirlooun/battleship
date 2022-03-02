using DAL.DAL.Entities.Enums;
using DAL.DAL.Entities.Enums.Rules;
using DAL.DTO;
using DAL.Services;

namespace DAL
{
    public class HitResponse
    {
        private bool IsHit { get; }
        private string? BoatName { get; }
        private bool IsDestroyed { get; }
        private bool IsSameCell { get; }

        public HitResponse(bool isHit, string? boatName, bool isDestroyed, bool isSameCell)
        {
            IsHit = isHit;
            BoatName = boatName;
            IsDestroyed = isDestroyed;
            IsSameCell = isSameCell;
        }

        public HitResponse(string boatName, int boatHp)
        {
            IsHit = true;
            BoatName = boatName;
            IsDestroyed = boatHp == 0;
        }

        public HitResponse(ECellState hitType)
        {
            IsSameCell = hitType == ECellState.Hit;
        }
        public bool IsSamePlayerMove(EHitContinuousMove hitContinuesMove)
        {
            return (IsHit || IsDestroyed || IsSameCell) && hitContinuesMove == EHitContinuousMove.HitContinuousMove;
        }
        public string GetMessage()
        {

            string message;

            if (IsDestroyed)
            {
                message = $"You destroyed {BoatName}";
            } else if (IsSameCell)
            {
                message = "You have already hit this cell!";
            } else if (IsHit)
            {
                message = "You hit a boat!";
            }
            else
            {
                message = "You missed";
            }

            return message;
        }

        public HitResponseDto GetDto()
        {
            return new HitResponseDto
            {
                IsDestroyed = IsDestroyed,
                IsSameCell = IsSameCell,
                IsHit = IsHit,
                BoatName = BoatName
            };
        }
    }
}
