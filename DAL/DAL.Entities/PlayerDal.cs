using System.Collections.Generic;

namespace DAL.DAL.Entities
{
    public class PlayerDal
    {
        private List<BoatDal>? _boats;

        private readonly List<LocationPointDal> _madeHits = new();
        public string Name { get; }
        public PlayerDal(string name)
        {
            Name = name;
        }
        
        public PlayerDal(string name, List<BoatDal> boats, List<LocationPointDal> hits)
        {
            Name = name;
            _boats = boats;
            _madeHits = hits;
        }

        public void SetPlayerBoats(List<BoatDal> boats)
        {
            _boats = boats;
        }

        public List<BoatDal> GetBoats()
        {
            return _boats!;
        }

        public List<LocationPointDal> GetHits()
        {
            return _madeHits;
        }

        public void AddHitToHistory(LocationPointDal locationPoint)
        {
            _madeHits.Add(locationPoint);
        }
    }
}
