using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTree.Models
{
    /// <summary>
    /// Consist the logic behind a team of Employees with Employee Leader and project assigned
    /// </summary>
    public class Team
    {
        public Team()
        {
            this.Members = new HashSet<Employee>();
            //this.Projects = new HashSet<Project>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Team name")]
        public string Name { get; set; }

        [Required]
        public DeliveryUnit Delivery { get; set; }

        [ForeignKey("Leader")]
        public int LeaderId { get; set; }

        [Display(Name = "Leader name")]
        public virtual Employee Leader { get; set; }

        [Display(Name = "Team members")]
        public virtual ICollection<Employee> Members { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        public string NameAndDelivery
        {
            get
            {
                return Name + ", " + Delivery;
            }
        }

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
            var team = other as Team;
            if (other == null || team == null)
            {
                return false;
            }

            return this.Id.Equals(team.Id);
        }
    }
}
