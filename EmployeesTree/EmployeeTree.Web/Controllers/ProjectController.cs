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
            foreach (var team in project.Teams)
            {
                team.ProjectId = null;
            }
            context.Projects.Remove(project);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Project/Create
        public ActionResult CreateWithTeams()
        {
            ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithTeams([Bind(Include = "Id,Name,Delivery, Teams")]Project projectModel)
        {
            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Teams")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (projectModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            var projectToSave = new Project();
            projectToSave.Name = projectModel.Name;
            projectToSave.Delivery = projectModel.Delivery;

            if (projectModel.Teams != null)
            {
                foreach (var team in projectModel.Teams)
                {
                    var teamToAdd = context.Teams.Find(team.Id);
                    projectToSave.Teams.Add(teamToAdd);
                }
            }

            context.Projects.Add(projectToSave);
            context.SaveChanges();
            return RedirectToAction("Index");

        }


        // GET: Project/Edit/5
        public ActionResult EditWithTeams(int? id)
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

            var projectToEdit = new ProjectWithTeamsViewModel();
            projectToEdit.Id = project.Id;
            projectToEdit.Delivery = project.Delivery;
            projectToEdit.Name = project.Name;
            projectToEdit.Teams = project.Teams.ToList();

            ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
            return View(projectToEdit);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWithTeams([Bind(Include = "Id,Name,Delivery,Teams")] ProjectWithTeamsViewModel projectModel)
        {
            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Teams")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (projectModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            var projectEditted = context.Projects.Find(projectModel.Id);
            projectEditted.Name = projectModel.Name;
            projectEditted.Delivery = projectModel.Delivery;

            foreach (var team in projectEditted.Teams)
            {
                team.ProjectId = null;
            }
            //projectEditted.Teams.Clear();

            if (projectModel.Teams != null)
            {
                foreach (var team in projectModel.Teams)
                {
                    var teamToAdd = context.Teams.Find(team.Id);
                    projectEditted.Teams.Add(teamToAdd);
                }
                
            }

            //context.Projects.Add(projectEditted);
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
