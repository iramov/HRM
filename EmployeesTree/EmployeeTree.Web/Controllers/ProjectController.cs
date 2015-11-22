namespace EmployeeTree.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using EmployeeTree.Data;
    using EmployeeTree.Models;
    using EmployeeTree.Web.ViewModels;
    using System;

    public class ProjectController : Controller
    {
        private IEmployeeDbContext context;

        public ProjectController(EmployeeDbContext context)
        {
            this.context = context;
        }

        // GET: Project
        public ActionResult Index(bool isAscending = false, string orderByColumn = null)
        {
            ViewBag.IsAscending = isAscending;
            var projects = context.Projects.ToList();
            var orderFunc = GetOrderFunction(orderByColumn);
            var projectsSorted = isAscending ? projects.OrderBy(orderFunc) : projects.OrderByDescending(orderFunc);
            return View(projectsSorted.ToList());
        }


        private Func<Project, object> GetOrderFunction(string orderByColumn)
        {
            Func<Project, object> orderFunc;
            switch (orderByColumn)
            {
                case "Name":
                    orderFunc = project => project.Name;
                    break;
                case "Delivery":
                    orderFunc = project => project.Delivery;
                    break;
                default:
                    orderFunc = project => project.Id;
                    break;
            }

            return orderFunc;
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
            if (project.Teams != null)
            {
                foreach (var team in project.Teams)
                {
                    team.ProjectId = null;
                }
            }

            context.Projects.Remove(project);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery, Teams")]ProjectWithTeamsViewModel projectModel)
        {
            //Validations
            if (projectModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }

            var disinctTeams = projectModel.Teams.Distinct();
            if (disinctTeams.Count() < projectModel.Teams.Count)
            {
                ModelState.AddModelError("Teams", "Each team may exists only once in a project.");
            }

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Teams")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            //Creating new project and filling its props
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

            var projectToEdit = new ProjectWithTeamsViewModel();
            projectToEdit.Id = project.Id;
            projectToEdit.Delivery = project.Delivery;
            projectToEdit.Name = project.Name;
            projectToEdit.Teams = project.Teams.ToList();

            var allTeams = context.Teams.ToList();
            var subtractTeams = allTeams.Except(projectToEdit.Teams).ToList();
            ViewBag.Teams = new SelectList(subtractTeams, "Id", "NameAndDelivery");
            return View(projectToEdit);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery,Teams")] ProjectWithTeamsViewModel projectModel)
        {
            //Validations
            if (projectModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }
            var disinctTeams = projectModel.Teams.Distinct();
            if (disinctTeams.Count() < projectModel.Teams.Count)
            {
                ModelState.AddModelError("Teams", "Each team may exists only once in a project.");
            }

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Teams")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Teams = new SelectList(context.Teams, "Id", "NameAndDelivery");
                return View(projectModel);
            }

            //Getting the edittedProject from the Db and setting its props
            var projectEditted = context.Projects.Find(projectModel.Id);
            projectEditted.Name = projectModel.Name;
            projectEditted.Delivery = projectModel.Delivery;

            if (projectModel.Teams != null)
            {
                //Subtraction the Old from New team so we can get the teams who are removed from the project

                var subractOldFromNewTeams = projectEditted.Teams.Except(projectModel.Teams);
                if (subractOldFromNewTeams.Any())
                {
                    foreach (var team in subractOldFromNewTeams)
                    {
                        var teamRemoveProject = context.Teams.Find(team.Id);
                        teamRemoveProject.ProjectId = null;
                    }
                }


                //Subtraction the New from Old team so we can get the teams who are added to the project
                var subractNewFromOldTeams = projectModel.Teams.Except(projectEditted.Teams);
                if (subractNewFromOldTeams.Any())
                {
                    foreach (var team in subractNewFromOldTeams)
                    {
                        var teamAddProject = context.Teams.Find(team.Id);
                        teamAddProject.ProjectId = projectModel.Id;
                    }
                }

            }


            //if (projectModel.Teams != null)
            //{
            //    foreach (var team in projectEditted.Teams)
            //    {
            //        team.ProjectId = null;
            //    }
            //    foreach (var team in projectModel.Teams)
            //    {
            //        var teamToAdd = context.Teams.Find(team.Id);
            //        projectEditted.Teams.Add(teamToAdd);
            //    }

            //}

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
