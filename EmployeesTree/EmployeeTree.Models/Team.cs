using System;
using System.Collections.Generic;
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

        public string Name { get; set; }

        public DeliveryUnit Delivery { get; set; }

        [ForeignKey("Leader")]
        public int LeaderId { get; set; }

        public Employee Leader { get; set; }

        public virtual ICollection<Employee> Members { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
