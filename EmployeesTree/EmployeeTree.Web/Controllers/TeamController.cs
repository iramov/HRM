namespace EmployeeTree.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using EmployeeTree.Data;
    using EmployeeTree.Models;
    using EmployeeTree.Web.ViewModels;

    public class TeamController : Controller
    {
        private IEmployeeDbContext context;

        public TeamController(EmployeeDbContext context)
        {
            this.context = context;
        }

        // GET: Team
        public ActionResult Index()
        {
            var teams = context.Teams.Include(t => t.Leader).Include(t => t.Project);
            return View(teams.ToList());
        }

        // GET: Team/Details/5
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

        // GET: Team/Delete/5
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

        // POST: Team/Delete/5
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

        // GET: Team/Create
        public ActionResult Create()
        {
            fillTheViewBags();
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId, Members")] TeamWithEmployeesViewModel teamModel)
        {
            if (teamModel.Members != null)
            {
                var disinctEmployees = teamModel.Members.Distinct();
                if (disinctEmployees.Count() < teamModel.Members.Count)
                {
                    ModelState.AddModelError("", "Each employee may exists only once in a team.");
                }
            }
            if (teamModel.LeaderId != null)
            {
                var leader = context.Employees.Find(teamModel.LeaderId);
                if (teamModel.Members != null)
                {
                    foreach (var member in teamModel.Members)
                    {
                        if (member.Position > leader.Position)
                        {
                            ModelState.AddModelError("", "The leader of the team must be the employee with the highest position.");
                        }
                        if (member.Id == teamModel.LeaderId)
                        {
                            ModelState.AddModelError("LeaderId", "The leader cannot be duplicated as a member of the team");
                        }
                    }
                }

            }
            else
            {
                ModelState.AddModelError("LeaderId", "Leader of the team is required.");
            }

            if (teamModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Members")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                fillTheViewBags();
                return View(teamModel);
            }

            //Creating a team and filling up its props
            var team = new Team();
            team.Name = teamModel.Name;
            team.Delivery = teamModel.Delivery;
            team.LeaderId = teamModel.LeaderId;
            team.ProjectId = teamModel.ProjectId;

            //Changing employees Manager and Delivery to be the same as their current team and after that adding them in the team
            foreach (var employee in teamModel.Members)
            {
                var teamMember = context.Employees.Find(employee.Id);
                teamMember.ManagerId = team.LeaderId;
                teamMember.Delivery = team.Delivery;
                team.Members.Add(teamMember);
            }

            context.Teams.Add(team);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int id)
        {
            var teamEdit = context.Teams.Find(id);
            //Creating TeamModelView and filling it up
            var teamView = new TeamWithEmployeesViewModel();
            teamView.Name = teamEdit.Name;
            teamView.Id = teamEdit.Id;
            teamView.Members = teamEdit.Members.ToList();
            teamView.LeaderId = teamEdit.LeaderId;
            teamView.ProjectId = teamEdit.ProjectId;
            teamView.Delivery = teamEdit.Delivery;

            fillTheViewBagsWithSelected(teamView);
            return View(teamView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Delivery,LeaderId,ProjectId, Members")] TeamWithEmployeesViewModel teamModel)
        {
            if (teamModel.Members != null)
            {
                var disinctEmployees = teamModel.Members.Distinct();
                if (disinctEmployees.Count() < teamModel.Members.Count)
                {
                    ModelState.AddModelError("", "Each employee may exists only once in a team.");
                }
            }
            if (teamModel.LeaderId != null)
            {
                var leader = context.Employees.Find(teamModel.LeaderId);
                if (teamModel.Members != null)
                {
                    foreach (var member in teamModel.Members)
                    {
                        if (member.Position > leader.Position)
                        {
                            ModelState.AddModelError("", "The leader of the team must be the employee with the highest position.");
                        }
                        if (member.Id == teamModel.LeaderId)
                        {
                            ModelState.AddModelError("LeaderId", "The leader cannot be duplicated as a member of the team");
                        }
                    }
                }

            }
            else
            {
                ModelState.AddModelError("LeaderId", "Leader of the team is required.");
            }

            if (teamModel.Delivery == 0)
            {
                ModelState.AddModelError("Delivery", "Delivery field is required");
            }

            var modelStateErrors = this.ModelState.Values.SelectMany(m => m.Errors);
            var errors = ModelState.Where(m => m.Key.Contains("Members")).Select(m => m.Key);
            foreach (var error in errors)
            {
                ModelState[error].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {

                fillTheViewBagsWithSelected(teamModel);
                return View(teamModel);
            }

            //taking the current team from the context and updating its fields
            var teamEditted = context.Teams.Find(teamModel.Id);
            teamEditted.Name = teamModel.Name;
            teamEditted.LeaderId = teamModel.LeaderId;
            teamEditted.ProjectId = teamModel.ProjectId;
            teamEditted.Delivery = teamModel.Delivery;

            if (teamModel.Members != null)
            {
                //Subtraction the Old from New team so we can get the teams who are removed from the project
                var subractOldFromNewMembers = teamEditted.Members.Except(teamModel.Members);
                foreach (var memberToRemove in subractOldFromNewMembers)
                {
                    //var employeeDeleteTeam = context.Employees.Find(memberToRemove.Id);
                    memberToRemove.TeamId = null;
                    memberToRemove.ManagerId = null;
                }

                //Subtraction the New from Old team so we can get the teams who are added to the project
                var subractNewFromOldMemebers = teamModel.Members.Except(teamEditted.Members);
                foreach (var memberToAdd in subractNewFromOldMemebers)
                {
                    var employeeToAdd = context.Employees.Find(memberToAdd.Id);
                    employeeToAdd.ManagerId = teamEditted.LeaderId;
                    employeeToAdd.Delivery = teamEditted.Delivery;
                    //employeeToAdd.TeamId = teamModel.Id;
                    teamEditted.Members.Add(employeeToAdd);
                }
            }


            //if (teamModel.Members != null)
            //{
            //    // Remove employees from the currently Editted team
            //    var employeesToRemove = teamEditted.Members.ToList();

            //    foreach (var membersToRemove in employeesToRemove)
            //    {
            //        var employeeDeleteTeam = context.Employees.Find(membersToRemove.Id);
            //        employeeDeleteTeam.TeamId = null;
            //        employeeDeleteTeam.ManagerId = null;
            //    }

            //    // Add the new employees in the Editted team
            //    //teamEditted.Members = teamModel.Members;

            //    foreach (var member in teamModel.Members)
            //    {
            //        var employeeToAdd = context.Employees.Find(member.Id);
            //        employeeToAdd.ManagerId = teamEditted.LeaderId;
            //        employeeToAdd.Delivery = teamEditted.Delivery;
            //        //employeeToAdd.TeamId = teamModel.Id;
            //        teamEditted.Members.Add(employeeToAdd);
            //    }
            //}
            

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Fill the ViewBags with Leaders("LeadersId"), Projects("ProjectsId") and Employees("FreeEmployees")
        /// </summary>
        private void fillTheViewBags()
        {
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));
            var freeEmployees = context.Employees.Where(e => e.TeamId == null);

            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FullNameAndEmail");
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name");
            ViewBag.FreeEmployees = new SelectList(freeEmployees, "Id", "FullNameAndEmail");
        }

        private void fillTheViewBagsWithSelected(TeamWithEmployeesViewModel teamView)
        {
            var freeEmployees = context.Employees.Where(e => e.TeamId == null);
            var freeLeaders = context.Employees.Where(e => e.Position > Position.TeamLeader || (e.Position == Position.TeamLeader && (e.TeamId == null)));

            ViewBag.LeaderId = new SelectList(freeLeaders, "Id", "FullNameAndEmail", teamView.LeaderId);
            ViewBag.ProjectId = new SelectList(context.Projects, "Id", "Name", teamView.ProjectId);
            ViewBag.FreeEmployees = new SelectList(freeEmployees, "Id", "FullNameAndEmail", teamView.Members.Select(m => m.Id));
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
