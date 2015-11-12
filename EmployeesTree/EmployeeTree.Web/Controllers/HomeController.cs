namespace EmployeeTree.Web.Controllers
{
    using EmployeeTree.Data;
    using EmployeeTree.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    
    public class HomeController : Controller
    {
        private EmployeeDbContext context = new EmployeeDbContext();

        public ActionResult Index()
        {
            //Hardcoding some data so the base is initialized
            Employee employee = new Employee();
            employee.FirstName = "Smilen";
            employee.LastName = "Petrov";
            employee.Position = Position.Senior;

            Employee employee3 = new Employee();
            employee3.FirstName = "Kamen";
            employee3.LastName = "Peolk";
            employee3.Position = Position.Junior;

            Employee employee2 = new Employee();
            employee2.FirstName = "Pesho";
            employee2.LastName = "Georgiev";
            employee2.Position = Position.TeamLeader;


            context.Employees.Add(employee);
            context.Employees.Add(employee3);
            context.Employees.Add(employee2);
            
            context.SaveChanges();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}