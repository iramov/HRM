namespace EmployeeTree.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using EmployeeTree.Data;
    using EmployeeTree.Models;
    using EmployeeTree.Web.ViewModels;

    public class EmployeeController : Controller
    {
        private IEmployeeDbContext context;

        public EmployeeController(EmployeeDbContext context)
        {
            this.context = context;
        }

        // GET: Employee
        public ActionResult Index(bool isAscending = false, string orderByColumn = null)
        {
            ViewBag.IsAscending = isAscending;
            var employees = context.Employees.Include(e => e.Manager); //.Include(e => e.Team)
            var employeesList = new List<Employee>(employees);
            //Getting the lambda expression for the OrderBy func down below
            var orderFunc = GetOrderFunction(orderByColumn);
            //sorting the list with employees and giving it to the view
            var employeesSorted = isAscending ? employeesList.OrderBy(orderFunc) : employeesList.OrderByDescending(orderFunc);
            return View(employeesSorted);
        }

        private Func<Employee, object> GetOrderFunction(string orderByColumn)
        {
            Func<Employee, object> orderFunc;
            switch (orderByColumn)
            {
                case "FirstName":
                    orderFunc = employee => employee.FirstName;
                    break;
                case "LastName":
                    orderFunc = employee => employee.LastName;
                    break;
                case "Position":
                    orderFunc = employee => employee.Position;
                    break;
                case "Delivery":
                    orderFunc = employee => employee.Delivery;
                    break;
                case "Email":
                    orderFunc = employee => employee.Email;
                    break;
                case "Manager":
                    orderFunc = employee => employee.ManagerId;
                    break;
                //case "Team":
                //    orderFunc = employee => employee.TeamId;
                //    break;
                default:
                    orderFunc = employee => employee.Id;
                    break;
            }

            return orderFunc;
        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employees = context.Employees.Include(e => e.Address);
            var employeeView = employees.FirstOrDefault(e => e.Id == id);
            if (employeeView == null)
            {
                return HttpNotFound();
            }
            return View(employeeView);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            fillTheViewBags();
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Position,Delivery,Salary,WorkPlace,Email,CellNumber,Address,ManagerId")] Employee employeeModel)
        {
            //Validations
            if (employeeModel.Position == 0)
            {
                ModelState.AddModelError("Position", "The position field is required");
            }
            if (employeeModel.Delivery == 0 && employeeModel.Position != Position.CEO)
            {
                ModelState.AddModelError("Delivery", "The delivery field is required");
            }
            if (employeeModel.ManagerId != null)
            {
                var manager = context.Employees.Find(employeeModel.ManagerId);
                if (employeeModel.Position > manager.Position)
                {
                    ModelState.AddModelError("", "The employee cannot have manager with lower position then his");
                }
            }

            if (!ModelState.IsValid)
            {
                fillTheViewBags(employeeModel);
                return View(employeeModel);
            }

            context.Employees.Add(employeeModel);
            context.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employees = context.Employees.Include(e => e.Address);
            var employeeView = employees.FirstOrDefault(e => e.Id == id);
            if (employeeView == null)
            {
                return HttpNotFound();
            }
            fillTheViewBags(employeeView);
            return View(employeeView);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Position,Delivery,Salary,WorkPlace,Email,Address,CellNumber,ManagerId,TeamId")] Employee employeeModel)
        {
            if (employeeModel.Position == 0)
            {
                ModelState.AddModelError("Position", "The position field is required");
            }
            if (employeeModel.Delivery == 0 && employeeModel.Position != Position.CEO)
            {
                ModelState.AddModelError("Delivery", "The delivery field is required");
            }
            if (employeeModel.ManagerId != null)
            {
                var manager = context.Employees.Find(employeeModel.ManagerId);
                if (employeeModel.Position > manager.Position)
                {
                    ModelState.AddModelError("", "The employee cannot have manager wiht lower position then his");
                }

            }
            if (!ModelState.IsValid)
            {
                fillTheViewBags(employeeModel);
                return View(employeeModel);
            }

            context.Entry(employeeModel).State = EntityState.Modified;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employees = context.Employees.Include(e => e.Address);
            var employeeView = employees.FirstOrDefault(e => e.Id == id);
            if (employeeView == null)
            {
                return HttpNotFound();
            }
            return View(employeeView);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var employeeToDelete = context.Employees.Find(id);
            if (employeeToDelete.Teams != null)
            {
                //Setting teams to static variable cos if we delete a team the count is changed dynamically
                var teamsToDelete = employeeToDelete.Teams.ToList();
                foreach (var team in teamsToDelete)
                {
                    if (team.LeaderId == employeeToDelete.Id)
                    {
                        context.Teams.Remove(team);
                    }
                    else
                    {
                        team.Members.Remove(employeeToDelete);
                    }
                }
            }
            var employeesToEdit = context.Employees.Where(e => e.ManagerId == employeeToDelete.Id).ToList();
            foreach (var employee in employeesToEdit)
            {
                employee.ManagerId = null;
            }

            context.Employees.Remove(employeeToDelete);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EmployeeTeamPreview(int id)
        {
            var teamView = new EmployeeTeamViewModel();
            var teamMember = context.Employees.Find(id);

            if (teamMember.Teams.Count != 0)
            {
                //var team = context.Teams.Find(teamMember.TeamId);
                var teams = teamMember.Teams.ToList();

                teamView.Name = teams[0].Name;
                teamView.Project = teams[0].Project;
                teamView.Members = teams[0].Members;
                teamView.Delivery = teams[0].Delivery;

                var teamLeaderPosition = teams[0].Leader.Position;
                switch (teamLeaderPosition)
                {
                    case Position.TeamLeader:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    case Position.ProjectManager:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    case Position.DeliveryDirector:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    case Position.CEO:
                        teamView.TeamLeader = teams[0].Leader;
                        break;
                    default:
                        break;
                }

                if (teamView.TeamLeader.Manager != null)
                {
                    teamView.ProjectManager = teamView.TeamLeader.Manager;
                    if (teamView.ProjectManager.Manager != null)
                    {
                        teamView.DeliveryDirector = teamView.ProjectManager.Manager;
                        if (teamView.DeliveryDirector.Manager != null)
                        {
                            teamView.CEO = teamView.DeliveryDirector.Manager;
                        }
                    }
                }

            }

            return View(teamView);
        }

        /// <summary>
        /// Fill the ViewBags with Managers("ManagerId"), team to be member("AsMemberTeamId") and teams to be leader("AsLeaderTeamId")
        /// </summary>
        private void fillTheViewBags()
        {
            var managers = context.Employees.Where(e => e.Position >= Position.TeamLeader);

            ViewBag.ManagerId = new SelectList(managers, "Id", "FullNameAndEmail");
            //ViewBag.TeamId = new SelectList(context.Teams, "Id", "NameAndDelivery");
        }

        /// <summary>
        /// Fill the ViewBags with Managers("ManagerId"), team to be member("AsMemberTeamId") and teams to be leader("AsLeaderTeamId") and selected indexes
        /// </summary>
        /// 
        private void fillTheViewBags(Employee employee)
        {
            var managers = context.Employees.Where(e => e.Position >= Position.TeamLeader);

            ViewBag.ManagerId = new SelectList(managers, "Id", "FullNameAndEmail", employee.ManagerId);
            //ViewBag.TeamId = new SelectList(context.Teams, "Id", "NameAndDelivery", employee.TeamId);
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
