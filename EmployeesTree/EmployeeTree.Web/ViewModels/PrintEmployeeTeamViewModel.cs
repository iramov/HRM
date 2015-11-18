using EmployeeTree.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeTree.Web.ViewModels
{
    public class PrintEmployeeTeamViewModel
    {
        [Display(Name = "Team name")]
        public string Name { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public virtual Project Project { get; set; }

        [Display(Name = "Team leader")]
        public virtual Employee TeamLeader { get; set; }

        [Display(Name = "Project manager")]
        public virtual Employee ProjectManager { get; set; }

        [Display(Name = "Delivery manager")]
        public virtual Employee DeliveryManager { get; set; }

        [Display(Name = "CEO")]
        public virtual Employee CEO { get; set; }

        [Display(Name = "Team members")]
        public virtual ICollection<Employee> Members { get; set; }

        
    }
}