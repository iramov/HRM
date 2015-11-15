using EmployeeTree.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeTree.Web.ViewModel
{
    public class TeamWithEmployeesViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        public int LeaderId { get; set; }

        [Required]
        [Display(Name = "Leader name")]
        public virtual Employee Leader { get; set; }

        [Display(Name = "Team members")]
        public virtual IList<Employee> Members { get; set; }

        public int? ProjectId { get; set; }

        [Display(Name = "Project assigned name")]
        public virtual Project Project { get; set; }
    }
}