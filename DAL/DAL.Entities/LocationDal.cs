namespace DAL.DAL.Entities
{
    public class LocationDal
    {
        public LocationDal(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public LocationDal(LocationDal location)
        {
            X = location.X;
            Y = location.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
