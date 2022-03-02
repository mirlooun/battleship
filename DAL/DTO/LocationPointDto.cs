using System;
using DAL.DAL.Entities.Enums;

namespace DAL.DTO
{
    public class LocationPointDto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ECellState PointState { get; set; }
    }
}
