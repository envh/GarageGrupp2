using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2._0.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string VehicleSort { get; set; }
        public string TransportMedium { get; set; }
        public int Wheels { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}