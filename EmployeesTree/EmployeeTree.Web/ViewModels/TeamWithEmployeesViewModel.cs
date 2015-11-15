using EmployeeTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeTree.Web.ViewModel
{
    public class TeamWithEmployeesViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public int LeaderId { get; set; }

        public virtual Employee Leader { get; set; }

        public virtual IList<Employee> Members { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}