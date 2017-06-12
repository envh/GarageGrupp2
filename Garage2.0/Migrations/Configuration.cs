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
            context.ParkedVehicles.AddOrUpdate(
              p => p.RegNo,
              new ParkedVehicle { Type = VehicleTypes.Car, RegNo = "ABC123", Model = "PV", Colour = "Red", Brand = "Volvo", AmountOfWheels = 4, CheckInTime = DateTime.Now },
              new ParkedVehicle { Type = VehicleTypes.Motorcycle, RegNo = "ABC124", Model = "Roadster 2000", Colour = "Blue", Brand = "Yamaha", AmountOfWheels = 2, CheckInTime = DateTime.Now },
              new ParkedVehicle { Type = VehicleTypes.Boat, RegNo = "BBC123", Model = "Seaking", Colour = "Black", Brand = "Yamaha", AmountOfWheels = 0, CheckInTime = DateTime.Now },
              new ParkedVehicle { Type = VehicleTypes.Bus, RegNo = "BBC124", Model = "Typisk SL buss", Colour = "Red", Brand = "Scania", AmountOfWheels = 6, CheckInTime = DateTime.Now },
              new ParkedVehicle { Type = VehicleTypes.Airplane, RegNo = "CBA123", Model = "Airboss", Colour = "Pink", Brand = "Volvo", AmountOfWheels = 3, CheckInTime = DateTime.Now}
            );
            
        }
    }
}
