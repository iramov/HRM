using EmployeeTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeTree.Web.ViewModel
{
    public class TeamWithEmployeesViewModel
    {
        public Team Team { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}