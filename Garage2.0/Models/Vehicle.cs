using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegNumber { get; set; }
        public string Colour { get; set; }
        public DateTime CheckInTime { get; set; }
        public int ParkingSlot { get; set; }
        public int MemberId { get; set; }
        public int VehicleTypeId { get; set; }

        public virtual Member Member { get; set; }
        public virtual VehicleType VehicleType { get; set; }
    }
}