namespace EmployeeTree.Data
{
    using EmployeeTree.Data.Migrations;
    using EmployeeTree.Models;
    using System.Data.Entity;

    public class EmployeeDbContext : DbContext, IEmployeeDbContext
    {
        public EmployeeDbContext()
            : base("EmployeeTreeConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmployeeDbContext, Configuration>());
        }

        //public static EmployeeDbContext Create()
        //{
        //    return new EmployeeDbContext();
        //}

        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Project> Projects { get; set; }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        //Old code for previous logic
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    //modelBuilder.Entity<Employee>()
        //    //        .HasOptional(e => e.Manager)
        //    //        .WithMany()
        //    //        .HasForeignKey(m => m.ManagerId);
        //}
    }
}
