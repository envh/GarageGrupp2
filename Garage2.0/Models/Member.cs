using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2._0.Models
{
    public  enum MemberStatus
    {
        Gold, Silver, Bronze
    }

    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InvoiceCost { get; set; }
        public int CurrentParkingCost { get; set; }
        public MemberStatus Status { get; set; }

        public  virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}