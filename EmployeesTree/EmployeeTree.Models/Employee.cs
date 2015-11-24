namespace EmployeeTree.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Each employee have information about his first and last name, position, salary, workplace, email, phone, home address and his manager.
    /// </summary>
    public class Employee
    {
        public Employee() 
        {
            this.Teams = new HashSet<Team>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public Position Position { get; set; }

        public DeliveryUnit Delivery { get; set; }

        public double Salary { get; set; }

        [Display(Name = "Workplace")]
        public string Workplace { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Email address is not valid")]
        public string Email { get; set; }

        [Display(Name = "Cell number")]
        public string CellNumber { get; set; }

        public Address Address { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        [Display(Name = "Manager name")]
        public virtual Employee Manager { get; set; }

        //[ForeignKey("Team")]
        //public int? TeamId { get; set; }

        //[Display(Name = "Team as member")]
        //public virtual Team Team { get; set; }+

        public virtual ICollection<Team> Teams { get; set; }


        public string FullNameAndEmail
        {
            get
            {
                return FirstName + " " + LastName + ", " + Email;
            }
        }

        public override int GetHashCode()
        {
            //if ((Id == null))
            //{
            //    return base.GetHashCode();
            //}
            return Id.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var employee = other as Employee;
            if (other == null || employee == null)
            {
                return false;
            }

            return this.Id.Equals(employee.Id);
        }
    }
}
