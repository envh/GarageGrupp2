namespace Garage2._0.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage2._0.DataAccessLayer.Garage2_0Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Garage2._0.DataAccessLayer.Garage2_0Context";
        }

        protected override void Seed(Garage2._0.DataAccessLayer.Garage2_0Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Members.AddOrUpdate(
              p => p.Name,
                new Member { Name = "Kalle Johansson", InvoiceCost = 0, CurrentParkingCost = 52, Status = MemberStatus.Gold },
                new Member { Name = "Nisse Karlsson", InvoiceCost  = 31, CurrentParkingCost = 0, Status = MemberStatus.Bronze },
                new Member { Name = "Lisa Nilsson", InvoiceCost = 0, CurrentParkingCost = 12, Status = MemberStatus.Silver }
            );

            context.SaveChanges();

            context.VehicleTypes.AddOrUpdate(
              p => p.VehicleSort,
                new VehicleType { VehicleSort = "Car", TransportMedium = "Road", Wheels = 4 },
                new VehicleType { VehicleSort = "Boat", TransportMedium = "Water", Wheels = 0},
                new VehicleType { VehicleSort = "ComAirPlane", TransportMedium = "Air", Wheels  = 8}
            );

            context.SaveChanges();

            context.Vehicles.AddOrUpdate(
              p => p.RegNumber,
                new Vehicle { RegNumber = "ABC 123", Colour = "Red", CheckInTime = DateTime.Now, ParkingSlot = 1, MemberId = 1, VehicleTypeId = 1, },
                new Vehicle { RegNumber = "DFR 125", Colour = "Blue", CheckInTime = DateTime.Now, ParkingSlot = 2, MemberId = 2, VehicleTypeId = 2, },
                new Vehicle { RegNumber = "KLT 763", Colour = "Black", CheckInTime = DateTime.Now, ParkingSlot = 6, MemberId = 1, VehicleTypeId = 2, }
            );

            context.SaveChanges();

        }
    }
}
