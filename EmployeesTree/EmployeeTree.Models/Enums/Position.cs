namespace EmployeeTree.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public  enum Position
    {
        Unknown,
        Trainee,
        Junior,
        Intermediate,
        Senior,

        //[EnumMember(Value = "Team leader")]
        [Display(Name = "Team leader")]
        TeamLeader,

        //[EnumMember(Value = "Project manager")]
        [Display(Name = "Project manager")]
        ProjectManager,

        //[EnumMember(Value = "Delivery director")]
        [Display(Name = "Delivery director")]
        DeliveryDirector,

        CEO
    }
}
