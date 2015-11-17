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
        private IEmployeeDbContext context;

        public TeamsController(EmployeeDbContext context)
        {
            this.context = context;
        }

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
            fillTheViewBags();
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId")] Team team)
        {
            if (!ModelState.IsValid)
            {
                //ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName", team.LeaderId);
                //ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
                fillTheViewBags();
                return View(team);
            }
            context.Teams.Add(team);
            context.SaveChanges();
            return RedirectToAction("Index");
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
            //ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FirstName", team.LeaderId);
            //ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
            //fillTheViewBags();
            ViewBag.LeaderId = new SelectList(context.Employees, "Id", "FirstName", team.LeaderId);
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
            return View(team);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId")] Team team)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LeaderId = new SelectList(context.Employees, "Id", "FirstName", team.LeaderId);
                ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", team.ProjectId);
                //fillTheViewBags();
                return View(team);

            }
            context.Entry(team).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");

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
            foreach (var member in team.Members)
            {
                member.TeamId = null;
            }
            context.Teams.Remove(team);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Teams/Create
        public ActionResult CreateWithEmployees()
        {
            fillTheViewBags();
            return View();
        }

        public class EmployeeEqualityComparer : IEqualityComparer<Employee>
        {
            public bool Equals(Employee x, Employee y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Employee obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithEmployees([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId, Members")] TeamWithEmployeesViewModel model)
        {
            if (model.Members == null)
            {
                ModelState.AddModelError("Employees", "Please add employees");
                return View(model);
            }

            //var disinctEmployees = model.MemberIds.Distinct();
            //if (disinctEmployees.Count() < model.MemberIds.Count)
            //{
            //    ModelState.AddModelError("Employees", "Each employee may exists only once in a team.");
            //    return View(model);
            //}

            //foreach (var member in model.MemberIds)
            //{
            //    //var currentMember = context.Employees.Find(member.Id);
            //    //member = currentMember

            //    var currentMember = context.Employees.Find(member.Id);
            //    member.FirstName = currentMember.FirstName;
            //    member.LastName = currentMember.LastName;
            //    member.Email = currentMember.Email;
            //}


            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);

            var errors = ModelState.Where(m => m.Key.Contains("Members")).Select(m => m.Key);

            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);

            if (!ModelState.IsValid)
            {
                fillTheViewBags();
                return View(model);
            }

            var team = new Team();
            team.Name = model.Name;
            team.Delivery = model.Delivery;
            team.LeaderId = model.LeaderId;
            team.ProjectId = model.ProjectId;

            foreach (var employee in model.Members)
            {
                var teamMember = context.Employees.Find(employee.Id);
                team.Members.Add(teamMember);
            }

            context.Teams.Add(team);

            //foreach (var employee in model.Members)
            //{
            //    var employeeModified = context.Employees.Find(employee.Id);
            //    employeeModified.ManagerId = model.LeaderId;
            //    employeeModified.Delivery = model.Delivery;
            //    employeeModified.TeamId = model.Id;
            //}



            //context.Teams.Add(model);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult EditWithEmployees(int id)
        {
            var teamEdit = context.Teams.Find(id);
            
            var teamView = new TeamWithEmployeesViewModel();
            teamView.Name = teamEdit.Name;
            teamView.Id = teamEdit.Id;
            teamView.Members = teamEdit.Members.ToList();
            teamView.LeaderId = teamEdit.LeaderId;
            teamView.ProjectId = teamEdit.ProjectId;
            teamView.Delivery = teamEdit.Delivery;
            //fillTheViewBags();
            
            var freeEmployees = context.Employees.Where(e => e.TeamId == null);
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            
            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FullNameAndEmail", teamView.LeaderId);
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", teamView.ProjectId);
            ViewBag.FreeEmployees = new SelectList(freeEmployees, "Id", "FullNameAndEmail", teamView.Members.Select(m => m.Id));

            return View(teamView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWithEmployees([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId, Members")] TeamWithEmployeesViewModel teamModel)
        {
            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);

            var errors = ModelState.Where(m => m.Key.Contains("Members")).Select(m => m.Key);

            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {

                var freeEmployees = context.Employees.Where(e => e.TeamId == null);
                var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));

                ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FullNameAndEmail", teamModel.LeaderId);
                ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name");
                ViewBag.FreeEmployees = new SelectList(freeEmployees, "Id", "FullNameAndEmail", teamModel.Members.Select(m => m.Id));
                return View(teamModel);
            }

            var teamEditted = context.Teams.Find(teamModel.Id);
            teamEditted.Name = teamModel.Name;
            teamEditted.LeaderId = teamModel.LeaderId;
            teamEditted.ProjectId = teamModel.ProjectId;
            teamEditted.Delivery = teamModel.Delivery;

            // Remove employees from the current Editted team
            var employeesToRemove = teamEditted.Members.ToList();

            foreach (var membersToRemove in employeesToRemove)
            {
                var employeeDeleteTeam = context.Employees.Find(membersToRemove.Id);
                employeeDeleteTeam.TeamId = null;
                employeeDeleteTeam.ManagerId = null;
            }

            // Add the new employees in the Editted team
            //teamEditted.Members = teamModel.Members;

            foreach (var member in teamModel.Members)
            {
                var employeeToAdd = context.Employees.Find(member.Id);
                employeeToAdd.TeamId = teamModel.Id;
                employeeToAdd.ManagerId = teamEditted.LeaderId;
                employeeToAdd.Delivery = teamEditted.Delivery;
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }


        private void fillTheViewBags()
        {
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            var freeEmployees = context.Employees.Where(e => e.TeamId == null);

            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FullNameAndEmail");
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name");
            ViewBag.FreeEmployees = new SelectList(freeEmployees, "Id", "FullNameAndEmail");
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
