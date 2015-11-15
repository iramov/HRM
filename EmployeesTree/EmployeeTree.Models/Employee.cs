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

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string CellNumber { get; set; }

        public Address Address { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        public virtual Employee Manager { get; set; }

        [ForeignKey("Team")]
        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }

        public string FullNameAndEmail
        {
            get
            {
                return FirstName + " " + LastName + ", " + Email;
            }
        }

        public override int GetHashCode()
        {
            if ((FirstName == null) || (LastName == null) || (Email == null))
            {
                return base.GetHashCode();
            }
            string stringRepresentation = FirstName + LastName + Email;
            return stringRepresentation.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var employee = other as Employee;
            if (other == null || employee == null)
            {
                return false;
            }

            return this.FirstName.Equals(employee.FirstName) &&
                    this.LastName.Equals(employee.LastName) &&
                    this.Email.Equals(employee.Email);
        }

        //public object Clone()
        //{
        //    return new Product
        //    {
        //        Id = this.Id,
        //        Name = this.Name,
        //        Customer = this.Customer,
        //        UniqueNumber = this.UniqueNumber,
        //        Comment = this.Comment,
        //        Parts = this.Parts,
        //        Date = this.Date,
        //        Week = this.Week,
        //        Part = this.Part
        //    };
        //}

    }
}
