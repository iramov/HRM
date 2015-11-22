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
            var employees = context.Employees.Include(e => e.Manager).Include(e => e.AsTeamLeader).Include(e => e.AsTeamMember);
            //employees = employees.ToList();
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
                case "AsTeamLeader":
                    orderFunc = employee => employee.AsTeamLeaderId;
                    break;
                case "AsTeamMember":
                    orderFunc = employee => employee.AsTeamMemberId;
                    break;
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
            var managerEmployees = context.Employees.Where(e => e.Position > Position.Senior);
            //var freeTeamLeaders = context.Employees.Where(tl => tl.TeamId == null && tl.Position == Position.TeamLeader);
            //managerEmployees.Concat(freeTeamLeaders);
            ViewBag.ManagerId = new SelectList(managerEmployees, "Id", "FirstName");
            ViewBag.TeamId = new SelectList(context.Teams, "Id", "Name");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Position,Delivery,Salary,WorkPlace,Email,CellNumber,Address,ManagerId,TeamId")] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
                ViewBag.AsTeamMemberId = new SelectList(context.Teams, "Id", "Name", employee.AsTeamMemberId);
                ViewBag.AsTeamLeaderId = new SelectList(context.Teams, "Id", "Name", employee.AsTeamLeaderId);
                return View(employee);
            }
            context.Employees.Add(employee);
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
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
            ViewBag.AsTeamMemberId = new SelectList(context.Teams, "Id", "Name", employee.AsTeamMemberId);
            ViewBag.AsTeamLeaderId = new SelectList(context.Teams, "Id", "Name", employee.AsTeamLeaderId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Position,Delivery,Salary,WorkPlace,Email,Address,CellNumber,ManagerId,TeamId")] Employee employee)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
                //ViewBag.TeamId = new SelectList(context.Teams, "Id", "Name", employee.TeamId);
                return View(employee);
            }
            context.Entry(employee).State = EntityState.Modified;
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
            if (employee.AsTeamLeaderId != null)
            {
                var employeeTeam = context.Teams.Find(employee.AsTeamLeaderId);
                //If the current employees is team leader the team must be deleted and all its members.TeamId must be nullified
                foreach (var member in employeeTeam.Members)
                {
                    member.AsTeamMemberId = null;
                    member.ManagerId = null;
                }
                context.Teams.Remove(employeeTeam);
            }
            if (employee.AsTeamMemberId != null)
            {
                var employeeTeam = context.Teams.Find(employee.AsTeamLeaderId);
                employeeTeam.Members.Remove(employee);
            }
            context.Employees.Remove(employee);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EmployeeTeamPreview(int id)
        {
            var teamView = new EmployeeTeamViewModel();
            var teamMember = context.Employees.Find(id);

            if (context.Teams.Any(e => e.Id == teamMember.AsTeamMemberId))
            {
                var team = context.Teams.Find(teamMember.AsTeamMemberId);

                teamView.Name = team.Name;
                teamView.Project = team.Project;
                teamView.Members = team.Members;
                teamView.Delivery = team.Delivery;

                var teamLeaderPosition = team.Leader.Position;
                switch (teamLeaderPosition)
                {
                    case Position.TeamLeader:
                        teamView.TeamLeader = team.Leader;
                        break;
                    case Position.ProjectManager:
                        teamView.TeamLeader = team.Leader;
                        break;
                    case Position.DeliveryDirector:
                        teamView.TeamLeader = team.Leader;
                        break;
                    case Position.CEO:
                        teamView.TeamLeader = team.Leader;
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

        private void fillTheViewBags(Employee employee)
        {
            ViewBag.ManagerId = new SelectList(context.Employees, "Id", "FirstName", employee.ManagerId);
            ViewBag.AsTeamMemberId = new SelectList(context.Teams, "Id", "Name", employee.AsTeamMemberId);
            ViewBag.AsTeamLeaderId = new SelectList(context.Teams, "Id", "Name", employee.AsTeamLeaderId);
            
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
