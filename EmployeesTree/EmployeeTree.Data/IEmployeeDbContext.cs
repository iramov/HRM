namespace EmployeeTree.Data
{
    using EmployeeTree.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IEmployeeDbContext
    {
        IDbSet<Employee> Employees { get; set; }

        IDbSet<Team> Teams { get; set; }

        IDbSet<Project> Projects { get; set; }

        void SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;


        void Dispose();
    }
}
