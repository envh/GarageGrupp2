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
    public class ParkedVehiclesController : Controller
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
            var ParkedVehicles = Filter(searchProp, searchValue);
            ParkedVehicles = Sort(orderBy, ParkedVehicles);
            return View(ParkedVehicles.ToList());
        }

        // GET: ParkedVehicles/Details/5
        public ActionResult Details(int? id, string searchProp, string searchValue)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            if (parkedVehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Create
        public ActionResult Create(string searchProp, string searchValue)
        {
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View();
        }

        // POST: ParkedVehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RegNo,Type,Colour,Brand,Model,AmountOfWheels")] ParkedVehicle parkedVehicle, string searchProp, string searchValue)
        {
            if (ModelState.IsValid)
            {
                parkedVehicle.CheckInTime = DateTime.Now;
                db.ParkedVehicles.Add(parkedVehicle);
                db.SaveChanges();
                return RedirectToAction("Index", new { searchProp , searchValue });
            }

            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            if (parkedVehicle == null)
            {
                return HttpNotFound();
            }
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RegNo,Type,Colour,Brand,Model,AmountOfWheels")] ParkedVehicle parkedVehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parkedVehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/Delete/5
        public ActionResult Delete(int? id, string searchProp, string searchValue)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
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
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            db.ParkedVehicles.Remove(parkedVehicle);
            db.SaveChanges();
            return RedirectToAction("Index", new { searchProp, searchValue });
        }

        public ActionResult Receipt(int? id, string searchProp, string searchValue)
        {
            ParkedVehicle parkedVehicle = db.ParkedVehicles.Find(id);
            ViewBag.SearchProp = searchProp;
            ViewBag.SearchValue = searchValue;
            return View(parkedVehicle);
        }

        private IQueryable<ParkedVehicle> Filter(string searchProp, string searchValue)
        {
            var Vehicles = db.ParkedVehicles.Select(e => e);
            if (searchProp == "RegNo") Vehicles = Vehicles.Where(e => e.RegNo == searchValue);
            else if (searchProp == "Type")
            {
                var vehicleType = (VehicleTypes)Enum.Parse(typeof(VehicleTypes), searchValue, true);
                Vehicles = Vehicles.Where(e => e.Type == vehicleType);

            }
            else if (searchProp == "Colour") Vehicles = Vehicles.Where(e => e.Colour == searchValue);
            else if (searchProp == "TimeParked")
            {
                var dateTime = int.Parse(searchValue);
                Vehicles = Vehicles.Where(e => (DbFunctions.DiffHours(e.CheckInTime, DateTime.Now)%24 >= dateTime));
            }
            return Vehicles;
        }
        private IQueryable<ParkedVehicle> Sort(string orderBy, IQueryable<ParkedVehicle> Vehicles)
        {
            if (orderBy == "RegNo") Vehicles = Vehicles.OrderBy(e => e.RegNo);
            else if (orderBy == "Type") Vehicles = Vehicles.OrderBy(e => e.Type);
            else if (orderBy == "Colour") Vehicles = Vehicles.OrderBy(e => e.Colour);
            else if (orderBy == "TimeParked") Vehicles = Vehicles.OrderBy(e => e.CheckInTime);
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
