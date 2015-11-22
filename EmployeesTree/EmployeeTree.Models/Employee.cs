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

        [Display(Name = "Work place")]
        public string WorkPlace { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Cell number")]
        public string CellNumber { get; set; }

        public Address Address { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        [Display(Name = "Manager name")]
        public virtual Employee Manager { get; set; }

        [ForeignKey("AsTeamLeader")]
        public int? AsLeaderTeamId { get; set; }

        [Display(Name = "As team leader assigned")]
        public virtual Team AsTeamLeader { get; set; }

        [ForeignKey("AsTeamMember")]
        public int? AsMemberTeamId { get; set; }

        [Display(Name = "As team member assigned")]
        public virtual Team AsTeamMember { get; set; }

        public string FullNameAndEmail
        {
            get
            {
                return FirstName + " " + LastName + ", " + Email;
            }
        }


        public override int GetHashCode()
        {
            if ((Id == null))
            {
                return base.GetHashCode();
            }
            //string stringRepresentation = FirstName + LastName + Email;
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
