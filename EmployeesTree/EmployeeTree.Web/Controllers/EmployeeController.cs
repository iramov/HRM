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
            var orderFunc = GetOrderFunction(orderByColumn);
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
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
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
            var employeeView = context.Employees.Find(id);
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
            //if (employeeModel.Position < Position.TeamLeader && employeeModel.AsLeaderTeamId != null)
            //{
            //    ModelState.AddModelError("", "To be leader of a team the employee position must be higher then senior");
            //}
            if (employeeModel.ManagerId != null)
            {
                var manager = context.Employees.Find(employeeModel.ManagerId);
                if (employeeModel.Position > manager.Position)
                {
                    ModelState.AddModelError("", "The employee cannot have manager wiht lower position then his");
                }
                
            }
            //if (employeeModel.AsLeaderTeamId != null && employeeModel.AsMemberTeamId != null)
            //    {
            //        if (employeeModel.AsLeaderTeamId == employeeModel.AsMemberTeamId)
            //        {
            //            ModelState.AddModelError("", "The employee cannot be duplicated as leader and member in a same team");
            //        }
            //    }
            if (!ModelState.IsValid)
            {
                fillTheViewBags(employeeModel);
                return View(employeeModel);
            }
            
            var employeeEditted = context.Employees.Find(employeeModel.Id);
            employeeEditted = employeeModel;
            //if (employeeModel.AsLeaderTeamId != null)
            //{
            //    var teamAsLeader = context.Teams.Find(employeeModel.AsLeaderTeamId);
            //    teamAsLeader.LeaderId = employeeNew.Id;
            //}
            //if (employeeModel.AsMemberTeamId != null)
            //{
            //    var teamAsMember = context.Teams.Find(employeeModel.AsMemberTeamId);
            //    teamAsMember.Members.Add(employeeNew);
            //}
            //context.Entry(employeeModel).State = EntityState.Modified;
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
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var employee = context.Employees.Find(id);
            //if (employee.AsLeaderTeamId != null)
            //{
            //    var employeeTeam = context.Teams.Find(employee.AsLeaderTeamId);
            //    //If the current employees is team leader the team must be deleted and all its members.TeamId-s must be nullified
            //    foreach (var member in employeeTeam.Members)
            //    {
            //        member.AsMemberTeamId = null;
            //        member.ManagerId = null;
            //    }
            //    context.Teams.Remove(employeeTeam);
            //}
            //if (employee.AsMemberTeamId != null)
            //{
            //    var employeeTeam = context.Teams.Find(employee.AsLeaderTeamId);
            //    employeeTeam.Members.Remove(employee);
            //}
            context.Employees.Remove(employee);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EmployeeTeamPreview(int id)
        {
            //var teamView = new EmployeeTeamViewModel();
            //var teamMember = context.Employees.Find(id);

            //if (context.Teams.Any(e => e.Id == teamMember.TeamId))
            //{
            //    var team = context.Teams.Find(teamMember.TeamId);

            //    teamView.Name = team.Name;
            //    teamView.Project = team.Project;
            //    teamView.Members = team.Members;
            //    teamView.Delivery = team.Delivery;

            //    var teamLeaderPosition = team.Leader.Position;
            //    switch (teamLeaderPosition)
            //    {
            //        case Position.TeamLeader:
            //            teamView.TeamLeader = team.Leader;
            //            break;
            //        case Position.ProjectManager:
            //            teamView.TeamLeader = team.Leader;
            //            break;
            //        case Position.DeliveryDirector:
            //            teamView.TeamLeader = team.Leader;
            //            break;
            //        case Position.CEO:
            //            teamView.TeamLeader = team.Leader;
            //            break;
            //        default:
            //            break;
            //    }

            //    if (teamView.TeamLeader.Manager != null)
            //    {
            //        teamView.ProjectManager = teamView.TeamLeader.Manager;
            //        if (teamView.ProjectManager.Manager != null)
            //        {
            //            teamView.DeliveryDirector = teamView.ProjectManager.Manager;
            //            if (teamView.DeliveryDirector.Manager != null)
            //            {
            //                teamView.CEO = teamView.DeliveryDirector.Manager;
            //            }
            //        }
            //    }

            //}

            return View();
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
