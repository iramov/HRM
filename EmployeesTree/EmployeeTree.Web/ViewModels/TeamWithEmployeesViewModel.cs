namespace EmployeeTree.Web.ViewModels
{
    using EmployeeTree.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TeamWithEmployeesViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        [Required]
        public int LeaderId { get; set; }

        
        [Display(Name = "Leader name")]
        public virtual Employee Leader { get; set; }

        [Display(Name = "Team members")]
        public virtual IList<Employee> Members { get; set; }

        public int? ProjectId { get; set; }

        [Display(Name = "Project assigned name")]
        public virtual Project Project { get; set; }
    }
}