using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Garage2._0.DataAccessLayer
{
    public class Garage2_0Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Garage2_0Context() : base("name=Garage2_0Context")
        {
        }

        public System.Data.Entity.DbSet<Garage2._0.Models.ParkedVehicle> ParkedVehicles { get; set; }
    }
}
