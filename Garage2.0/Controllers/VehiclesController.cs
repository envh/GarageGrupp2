using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2._0.DataAccessLayer;
using Garage2._0.Models;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

namespace Garage2._0.Controllers
{
    public class VehiclesController : Controller
    {
        private Garage2_0Context db = new Garage2_0Context();

        // GET: ParkedVehicles
        public ActionResult Index(string searchProp, string searchValue, string orderBy)
        {
            if (searchProp == null)
            {
                ViewBag.InsertLink = false;
            }
            else
            {
                ViewBag.InsertLink = true;
            }
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            ViewBag.orderBy = orderBy;
            var ParkedVehicles = Filter(searchProp, searchValue);
            ParkedVehicles = Sort(orderBy, ParkedVehicles);
            if (!ParkedVehicles.Any()) ViewBag.empty = true;
            else ViewBag.empty = false;
            ViewBag.Types = db.VehicleTypes.Select(t => t);
            ViewBag.Members = db.Members.Select(m => m);
            return View(ParkedVehicles.ToList());
        }

        public ActionResult FullView(string searchProp, string searchValue)
        {
            var ParkedVehicles = Filter(searchProp, searchValue);
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            ViewBag.Types = db.VehicleTypes.Select(t => t);
            return View(ParkedVehicles.ToList());
        }

        // GET: ParkedVehicles/Details/5
        public ActionResult Details(int? id, string searchProp, string searchValue)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle Vehicle = db.Vehicles.Find(id);
            if (Vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View(Vehicle);
        }

        // GET: ParkedVehicles/Create
        public ActionResult Create(string searchProp, string searchValue)
        {
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            var VehicleTypes = db.VehicleTypes.Select(t => t);
            var Members = db.Members.Select(m => m);
            ViewBag.VehicleTypes = VehicleTypes.ToList();
            ViewBag.Members = Members.ToList();
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RegNumber,Colour, VehicleTypeId, MemberId")] Vehicle Vehicle,string searchProp, string searchValue)
        {
            if (ModelState.IsValid)
            {
                Vehicle.CheckInTime = DateTime.Now;
                Vehicle.ParkingSlot = 0;

                var usedSlots = db.Vehicles.Select(v => v);
                for (int i = 1; i <= 25; i++)
                {
                    if (!usedSlots.Any(v => v.ParkingSlot == i) )
                    {
                        Vehicle.ParkingSlot = i;
                        i = 100;
                    }
                }

                if (Vehicle.ParkingSlot != 0)
                {
                    db.Vehicles.Add(Vehicle);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { searchProp, searchValue });
                }
                else
                {
                    ViewBag.ErrorGarageFull = "Sorry, the garage is currently full!";
                }
            }

            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            var VehicleTypes = db.VehicleTypes.Select(t => t);
            var Members = db.Members.Select(m => m);
            ViewBag.VehicleTypes = VehicleTypes.ToList();
            ViewBag.Members = Members.ToList();
            return View(Vehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle Vehicle = db.Vehicles.Find(id);
            if (Vehicle == null)
            {
                return HttpNotFound();
            }
            return View(Vehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RegNo,Type,Colour,Brand,Model,AmountOfWheels")] Vehicle Vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Vehicle);
        }

        // GET: ParkedVehicles/Delete/5
        public ActionResult Delete(int? id, string searchProp, string searchValue)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle parkedVehicle = db.Vehicles.Find(id);
            if (parkedVehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Delete/5
        [HttpPost, ActionName("Receipt")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string searchProp, string searchValue)
        {
            Vehicle Vehicle = db.Vehicles.Find(id);
            var member = Vehicle.Member;
            member.InvoiceCost += ViewBag.CheckedOutCost;
            
 
            db.Vehicles.Remove(Vehicle);
            db.SaveChanges();
            return RedirectToAction("Index", new { searchProp, searchValue });
        }

        public ActionResult Receipt(int? id, string searchProp, string searchValue)
        {
            Vehicle Vehicle = db.Vehicles.Find(id);
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View(Vehicle);
        }

        private IQueryable<Vehicle> Filter(string searchProp, string searchValue)
        {
            
            var Vehicles = db.Vehicles.Select(e => e);
            
            if (searchProp == "RegNumber") Vehicles = Vehicles.Where(e => e.RegNumber.Contains(searchValue));
            else if (searchProp == "Type") Vehicles = Vehicles.Where(e => e.VehicleType.VehicleSort == searchValue);
            else if (searchProp == "TimeParkedMore")
            {
                int dateTime;
                if(int.TryParse(searchValue, out dateTime)) Vehicles = Vehicles.Where(e => (DbFunctions.DiffHours(e.CheckInTime, DateTime.Now) >= dateTime));
            }
            else if (searchProp == "TimeParkedLess")
            {
                int dateTime;
                if(int.TryParse(searchValue, out dateTime)) Vehicles = Vehicles.Where(e => (DbFunctions.DiffHours(e.CheckInTime, DateTime.Now) <= dateTime));
            }
            else if (searchProp == "Member") Vehicles = Vehicles.Where(e => e.Member.Name.Contains(searchValue));
            else if (searchProp == "Membership") Vehicles = Vehicles.Where(e => e.Member.Status.ToString() == searchValue);
            //else if (searchProp == "CheckInTime") Vehicles = Vehicles.Where(e => e.CheckInTime == searchValue);
            else if (searchProp == "ParkingSlot") Vehicles = Vehicles.Where(e => e.ParkingSlot.ToString() == searchValue);
            else if (searchProp == "Terrain") Vehicles = Vehicles.Where(e => e.VehicleType.TransportMedium == searchValue);
            else if (searchProp == "Wheels") Vehicles = Vehicles.Where(e => e.VehicleType.Wheels.ToString() == searchValue);
            else if (searchProp == "Colour") Vehicles = Vehicles.Where(e => e.Colour == searchValue);
            return Vehicles;
        }
        private IQueryable<Vehicle> Sort(string orderBy, IQueryable<Vehicle> Vehicles)
        {
            if (orderBy == "RegNo") Vehicles = Vehicles.OrderBy(e => e.RegNumber);
            else if (orderBy == "RegNoDesc") Vehicles = Vehicles.OrderByDescending(e => e.RegNumber);
            else if (orderBy == "Type") Vehicles = Vehicles.OrderBy(e => e.VehicleType.VehicleSort);
            else if (orderBy == "TypeDesc") Vehicles = Vehicles.OrderByDescending(e => e.VehicleType.VehicleSort);
            else if (orderBy == "Member") Vehicles = Vehicles.OrderBy(e => e.Member.Name);
            else if (orderBy == "MemberDesc") Vehicles = Vehicles.OrderByDescending(e => e.Member.Name);
            else if (orderBy == "TimeParked") Vehicles = Vehicles.OrderBy(e => e.CheckInTime);
            else if (orderBy == "TimeParkedDesc") Vehicles = Vehicles.OrderByDescending(e => e.CheckInTime);
            else if (orderBy == "ParkingSlot") Vehicles = Vehicles.OrderBy(e => e.ParkingSlot);
            else if (orderBy == "ParkingSlotDesc") Vehicles = Vehicles.OrderByDescending(e => e.ParkingSlot);
            return Vehicles;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
