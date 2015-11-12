namespace EmployeeTree.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Each employee have information about his first and last name, position, salary, workplace, email, phone, home address and his manager.
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string LastName { get; set; }

        [Required]
        public Position Position { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public double Salary { get; set; }

        public string WorkPlace { get; set; }

        public string Email { get; set; }

        public string CellNumber { get; set; }

        public Address Address { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        public Employee Manager { get; set; }

        //[ForeignKey("Team")]
        //public int TeamId { get; set; }

        //public virtual Team Team { get; set; }
        
    }
}
