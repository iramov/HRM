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
using EmployeeTree.Web.ViewModels;

namespace EmployeeTree.Web.Controllers
{
    public class ProjectController : Controller
    {
        private IEmployeeDbContext context;

        public ProjectController(EmployeeDbContext context)
        {
            this.context = context;
        }

        // GET: Project
        public ActionResult Index()
        {
            return View(context.Projects.ToList());
        }

        // GET: Project/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = context.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery")] Project project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }
            context.Projects.Add(project);
            context.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = context.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery")] Project project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }
            context.Entry(project).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = context.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = context.Projects.Find(id);
            context.Projects.Remove(project);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Project/Create
        public ActionResult CreateWithTeams()
        {
            ViewBag.Teams = new SelectList(context.Teams, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithTeams([Bind(Include = "Id,Name,Delivery, Teams")] ProjectViewModel project)
        {
            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);

            var errors = ModelState.Where(m => m.Key.Contains("Teams")).Select(m => m.Key);

            if (!ModelState.IsValid)
            {
                return View(project);
            }
            //context.Projects.Add(project);
            //context.SaveChanges();
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
