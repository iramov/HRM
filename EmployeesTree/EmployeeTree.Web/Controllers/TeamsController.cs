namespace EmployeeTree.Web.Controllers
{
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
    using EmployeeTree.Web.ViewModel;

    public class TeamsController : Controller
    {
        private EmployeeDbContext context = new EmployeeDbContext();

        //public TeamsController(IEmployeeDbContext context)
        //{
        //    this.context = context;
        //}

        // GET: Teams
        public ActionResult Index()
        {
            var teams = context.Teams.Include(t => t.Leader).Include(t => t.Project);
            return View(teams.ToList());
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = context.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FullNameAndEmail");
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId")] Team team)
        {
            if (ModelState.IsValid)
            {
                context.Teams.Add(team);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName", team.LeaderId);
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
            return View(team);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = context.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName", team.LeaderId);
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId")] Team team)
        {
            if (ModelState.IsValid)
            {
                context.Entry(team).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName", team.LeaderId);
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
            return View(team);
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = context.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = context.Teams.Find(id);
            context.Teams.Remove(team);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Teams/Create
        public ActionResult CreateWithEmployees()
        {
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name");
            var freeEmployees = context.Employees.Where(e => e.TeamId == null);
            ViewBag.FreeEmployees = new SelectList(freeLeaders, "Id", "FullNameAndEmail");
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithEmployees(TeamWithEmployeesViewModel team)
        {
            //if (ModelState.IsValid)
            //{
            //    context.Teams.Add(team);
            //    context.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            //ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName", team.LeaderId);
            //ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);

            if (!ModelState.IsValid)
            {
                return View(team);
            }

            context.Teams.Add(team.Team);
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
