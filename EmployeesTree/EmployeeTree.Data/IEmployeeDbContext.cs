namespace EmployeeTree.Data
{
    using EmployeeTree.Models;
    using System.Data.Entity;

    public interface IEmployeeDbContext
    {
        IDbSet<Employee> Employees { get; set; }

        IDbSet<Team> Teams { get; set; }

        IDbSet<Project> Projects { get; set; }

        void SaveChanges();

    }
}
