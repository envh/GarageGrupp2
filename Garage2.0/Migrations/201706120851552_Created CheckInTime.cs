namespace Garage2._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedCheckInTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ParkedVehicles", "CheckInTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ParkedVehicles", "CheckInTime");
        }
    }
}
