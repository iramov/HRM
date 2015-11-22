using EmployeeTree.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeTree.Web.ViewModels
{
    public class ProjectWithTeamsViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        [Display(Name = "Team names")]
        public virtual IList<Team> Teams { get; set; }

        public override int GetHashCode()
        {
            if ((Id == null))
            {
                return base.GetHashCode();
            }
            return Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var project = other as Project;
            if (other == null || project == null)
            {
                return false;
            }

            return this.Id.Equals(project.Id);
        }
    }
}