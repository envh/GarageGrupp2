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

namespace Garage2._0.Controllers
{
    public class MembersController : Controller
    {
        private Garage2_0Context db = new Garage2_0Context();

        // GET: Members
        public ActionResult Index(string SearchValue, string orderBy)
        {
            var members = Filter(SearchValue);
            if (SearchValue == null)
            {
                ViewBag.InsertLink = false;
            }
            else
            {
                ViewBag.InsertLink = true;
            }
            ViewBag.orderBy = orderBy;
            members = Sort(orderBy, members);

            foreach (var member in members)
            {
                int totalcost = 0;
                foreach (var item in member.Vehicles)
                {
                    TimeSpan diff = DateTime.Now.Subtract(item.CheckInTime);

                    int kost1, kost4;
                    kost1 = kost4 = 0;


                    kost4 = diff.Days * 31;

                    if (2 <= diff.Hours)
                    {
                        kost1 = 12;
                        if (30 < (diff.Hours - 2) * 60 + diff.Minutes)
                        {
                            kost1 += 5;
                        }
                    }

                    if (12 <= diff.Hours)
                    {
                        kost1 = 27;
                        if (30 < (diff.Hours - 12) * 60 + diff.Minutes)
                        {
                            kost1 += 4;
                        }
                    }

                    totalcost += kost1 + kost4;
                }
                member.CurrentParkingCost = totalcost;
            }
            db.SaveChanges();
            return View(members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }

            int totalcost = 0;
            foreach (var item in member.Vehicles)
            {
                TimeSpan diff = DateTime.Now.Subtract(item.CheckInTime);

                int kost1, kost4;
                kost1 = kost4 = 0;


                kost4 = diff.Days * 31;

                if (2 <= diff.Hours)
                {
                    kost1 = 12;
                    if (30 < (diff.Hours - 2) * 60 + diff.Minutes)
                    {
                        kost1 += 5;
                    }
                }

                if (12 <= diff.Hours)
                {
                    kost1 = 27;
                    if (30 < (diff.Hours - 12) * 60 + diff.Minutes)
                    {
                        kost1 += 4;
                    }
                }

                totalcost += kost1 + kost4;
            }
            member.CurrentParkingCost = totalcost;
            db.SaveChanges();
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,InvoiceCost,CurrentParkingCost,Status")] Member member)
        {
            member.InvoiceCost = 0;
            member.CurrentParkingCost = 0;
            var memberUnique = !db.Members.Any(m => m.Name == member.Name);
            if (ModelState.IsValid && memberUnique)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (!memberUnique) ViewBag.ErrorMemberExists = "Member already exists";
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,InvoiceCost,CurrentParkingCost,Status")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private IQueryable<Member> Filter(string searchValue)
        {
            var members = db.Members.Select(m => m);
            if(!string.IsNullOrEmpty(searchValue)) members = members.Where(m => m.Name.Contains(searchValue));
            return members;
        }

        private IQueryable<Member> Sort(string orderBy, IQueryable<Member> members)
        {
            if (orderBy == "Name") members = members.OrderBy(m => m.Name);
            else if (orderBy == "NameDesc") members = members.OrderByDescending(m => m.Name);
            else if (orderBy == "InvoiceCost") members = members.OrderBy(m => m.InvoiceCost);
            else if (orderBy == "InvoiceCostDesc") members = members.OrderByDescending(m => m.InvoiceCost);
            else if (orderBy == "CurrentParkingCost") members = members.OrderBy(m => m.CurrentParkingCost);
            else if (orderBy == "CurrentParkingCostDesc") members = members.OrderByDescending(m => m.CurrentParkingCost);
            else if (orderBy == "Status") members = members.OrderBy(m => m.Status);
            else if (orderBy == "StatusDesc") members = members.OrderByDescending(m => m.Status);
            else if (orderBy == "VehiclesParked") members = members.OrderBy(m => m.Vehicles.Count);
            else if (orderBy == "VehiclesParkedDesc") members = members.OrderByDescending(m => m.Vehicles.Count);
            return members;
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
