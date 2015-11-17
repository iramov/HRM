namespace EmployeeTree.Web.ViewModels
{
    using EmployeeTree.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        [Display(Name = "Teams names")]
        public virtual IList<Team> Teams { get; set; }
    }
}