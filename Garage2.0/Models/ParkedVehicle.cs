using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2._0.Models
{
    public enum VehicleTypes
    {
        Airplane,
        Boat,
        Bus,
        Car,
        Motorcycle
    }

    public class ParkedVehicle
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public VehicleTypes Type { get; set; }
        public string Colour { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int AmountOfWheels { get; set; }
        public DateTime CheckInTime { get; set; }
    }
}