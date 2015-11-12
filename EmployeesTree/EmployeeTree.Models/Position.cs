namespace EmployeeTree.Models
{
    using System.Runtime.Serialization;

    public  enum Position
    {
        Unknown,
        Trainee,
        Junior,
        Intermediate,
        Senior,

        [EnumMember(Value = "Team leader")]
        TeamLeader,

        [EnumMember(Value = "Project manager")]
        ProjectManager,

        [EnumMember(Value = "Delivery manager")]
        DeliveryManager,

        CEO
    }
}
