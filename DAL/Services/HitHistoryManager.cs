using System.Collections.Generic;
using System.Linq;
using DAL.DAL.Entities;
using DAL.DAL.Entities.Enums;
using DAL.Helpers;

namespace DAL.Services
{
    public class HitHistoryManager
    {
        private readonly Stack<HitRecord> _hitHistory;

        public HitHistoryManager()
        {
            _hitHistory = new Stack<HitRecord>();
        }
        
        public HitHistoryManager(IEnumerable<HitRecord> hitHistory)
        {
            _hitHistory = new Stack<HitRecord>(hitHistory);
        }

        public void AddHitToHistory(HitRecord hitRecord)
        {
            _hitHistory.Push(hitRecord);
        }

        public Stack<HitRecord> GetHistory()
        {
            return _hitHistory;
        }

        public void RevertGameStateToSpecified(GameEngine gameEngine, HitRecord record)
        {
            var (p1, p2) = gameEngine.GetPlayers();

            var recordWas = false;
            
            while (!recordWas)
            {
                var rec = _hitHistory.Pop();

                if (rec.Equals(record))
                {
                    recordWas = true;
                }
                
                var curAttacker = rec.IsFirst ? p1 : p2;
                var curEnemy = !rec.IsFirst ? p1 : p2;

                var playerHit = curAttacker.GetHits().FirstOrDefault(hit => hit.Equals(rec.HitLocation));
                
                if (playerHit == null)
                {
                    curAttacker.AddHitToHistory(rec.HitLocation);
                }
                else
                {
                    curAttacker.GetHits().Remove(playerHit);
                }

                // if player hit a ship
                if (rec.HitType == ECellState.Miss) continue;
                var targetBoat = TargetBoatFinder.FindBoat(curEnemy.GetBoats(), rec.HitLocation);
                var location = targetBoat!
                    .Locations.First(pl => pl.X.Equals(rec.HitLocation.X) && pl.Y.Equals(rec.HitLocation.Y));
                location.PointState = ECellState.Ship;
            }

            gameEngine.NextMoveByFirst = _hitHistory.Count == 0 || _hitHistory.Peek().IsFirst;
        }
    }

    public class HitRecord
    {
        public bool IsFirst { get; set; }
        public LocationPointDal HitLocation { get; set; } = default!;
        public ECellState HitType { get; set; }
        public string Info { get; set; } = default!;
    }
}