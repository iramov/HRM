﻿namespace EmployeeTree.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Every team is assigned to a project with its props. (Potential for expanding the base logic of the Demo)
    /// </summary>
    public class Project
    {
        public Project()
        {
            this.Teams = new HashSet<Team>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
