using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeTree.Data;
using EmployeeTree.Models;

namespace EmployeeTree.Web.Controllers
{
    public class EmployeesController : Controller
    {
        //private IEmployeeDbContext context;

        //public EmployeesController(EmployeeDbContext context) 
        //{
        //    this.context = context;        
        //}

        private EmployeeDbContext context = new EmployeeDbContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = context.Employees.Include(e => e.Manager).Include(e => e.Team);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            var managerEmployees = context.Employees.Where(e => e.Position > Position.Senior);
            //var freeTeamLeaders = context.Employees.Where(tl => tl.TeamId == null && tl.Position == Position.TeamLeader);
            //managerEmployees.Concat(freeTeamLeaders);
            ViewBag.ManagerId = new SelectList(managerEmployees, "Id", "FirstName");
            ViewBag.TeamId = new SelectList(context.Teams, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Position,Delivery,Salary,WorkPlace,Email,CellNumber,ManagerId,TeamId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                context.Employees.Add(employee);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
            ViewBag.TeamId = new SelectList(context.Teams, "Id", "Name", employee.TeamId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
            ViewBag.TeamId = new SelectList(context.Teams, "Id", "Name", employee.TeamId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Position,Delivery,Salary,WorkPlace,Email,CellNumber,ManagerId,TeamId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                context.Entry(employee).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
            ViewBag.TeamId = new SelectList(context.Teams, "Id", "Name", employee.TeamId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = context.Employees.Find(id);
            context.Employees.Remove(employee);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
