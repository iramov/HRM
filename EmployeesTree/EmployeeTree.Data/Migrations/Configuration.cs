namespace EmployeeTree.Data.Migrations
{
    using EmployeeTree.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmployeeTree.Data.EmployeeDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }


        // Seed database 
        protected override void Seed(EmployeeDbContext context)
        {
            if (!context.Projects.Any())
            {
                SeedProjects(context);
            }
            if (!context.Employees.Any())
            {
                SeedEmployees(context);
            }
        }
        // Seed employees method
        private static void SeedEmployees(EmployeeDbContext context)
        {
            var employee = new List<Employee>()
            {
                new Employee
                {
                    FirstName = "Hristo",
                    LastName = " Antov",
                    Delivery = DeliveryUnit.Healthcare,
                    //ManagerId = 6,
                    Position = Position.Trainee,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "f0ri@abv.bg",
                    CellNumber = "0999 99 55 33",
                    Address = new Address(){
                        Street = "Manastirski livadi",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }
                },
                new Employee
                {
                    FirstName = "Iliqn",
                    LastName = " Ramov",
                    Position = Position.Trainee,
                    //ManagerId = 6,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "iliqn@abv.bg",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }
                },
                new Employee
                {
                    FirstName = "Damqn",
                    LastName = " Grancharov",
                    Delivery = DeliveryUnit.Healthcare,
                    Position = Position.Trainee,
                    //ManagerId = 6,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "damqn@abv.bg",
                    CellNumber = "0999 99 88 77",
                    Address = new Address(){
                        Street = "Center",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }
                },
                new Employee
                {
                    FirstName = "Boris",
                    LastName = " Borovski",
                    Position = Position.Trainee,
                    //ManagerId = 6,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "boris@abv.bg",
                    CellNumber = "0999 70 85 12",
                    Address = new Address(){
                        Street = "Center",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }},
                new Employee
                {
                    FirstName = "Mitko",
                    LastName = " Ivanov",
                    Position = Position.Trainee,
                    //ManagerId = 6,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "mitko@abv.bg",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }
                },
                new Employee
                {
                    FirstName = "Juliyan",
                    LastName = " Boyanov",
                    Position = Position.TeamLeader,
                    //ManagerId = 10,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 10000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "juliqn@gmail.bg",
                    Address = new Address(){
                        Street = "Center",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }
                },
                new Employee
                {
                    FirstName = "Desislav",
                    LastName = " Bonchev",
                    Position = Position.TeamLeader,
                    //ManagerId = 10,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "desislav@abv.bg",
                    CellNumber = "0979 423 432",
                    Address = new Address(){
                        Street = "Center",
                        City = "Sofia",
                        Country = "Bulgaria"
                        }
                },
                new Employee
                {
                    FirstName = "Vladimir",
                    LastName = " Granicki",
                    Position = Position.Junior,
                    //ManagerId = 9,
                    Delivery = DeliveryUnit.Healthcare,
                    Salary = 2500,
                    Workplace = "SoftServe Ukraine",
                    Email = "vladimir@abv.bg",
                    Address = new Address(){
                        Street = "Center",
                        City = "Lviv",
                        Country = "Ukraine"
                        }
                },
                new Employee
                {
                    FirstName = "Vitalii",
                    LastName = " Klichko",
                    Position = Position.TeamLeader,
                    //ManagerId = 10,
                    Delivery = DeliveryUnit.Entertainment,
                    Salary = 5000,
                    Workplace = "SoftServe Ukraine",
                    Email = "vitalii@gmail.bg",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Lviv",
                        Country = "Ukraine"
                        }
                },
                new Employee
                {
                    FirstName = "John",
                    LastName = " Johnson",
                    Position = Position.ProjectManager,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 15000,
                    Workplace = "SoftServe Ukraine",
                    Email = "john@gmail.com",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Lviv",
                        Country = "Ukraine"
                        }
                },
                new Employee
                {
                    FirstName = "Christopher",
                    LastName = " Baker",
                    Position = Position.ProjectManager,
                    //ManagerId = 12,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 15000,
                    Workplace = "SoftServe Ukraine",
                    Email = "christopher@gmail.com",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Lviv",
                        Country = "Ukraine"
                        }
                },
                new Employee
                {
                    FirstName = "Andriy",
                    LastName = " Stytsyuk",
                    Position = Position.DeliveryDirector,
                    //ManagerId = 13,
                    Delivery = DeliveryUnit.Finance,
                    Salary = 15000,
                    Workplace = "SoftServe Ukraine",
                    Email = "adriy@gmail.bg",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Lviv",
                        Country = "Ukraine"
                        }
                },
                new Employee
                {
                    FirstName = "Oleh",
                    LastName = " Denys",
                    Position = Position.CEO,
                    Salary = 20000,
                    Workplace = "SoftServe Ukraine",
                    Email = "oleh@gmail.com",
                    CellNumber = "0999 11 22 33",
                    Address = new Address(){
                        Street = "Center",
                        City = "Lviv",
                        Country = "Ukraine"
                        }
                }
            };

            foreach (var item in employee)
            {
                context.Employees.AddOrUpdate(item);
            }
            context.SaveChanges();
        }

        // Seed projects method
        private static void SeedProjects(EmployeeDbContext context)
        {
            var projects = new List<Project>{
                  new Project
                  {
                      Name = "HR management system",
                      Delivery = DeliveryUnit.Finance,
                      Description = "System that helps Hr's to manage employees in company"
                  },
                  new Project
                  {
                      Name = "New NASA operating system",
                      Delivery = DeliveryUnit.Entertainment,
                      Description = "It's top secret"
                  }};

            foreach (var item in projects)
            {
                context.Projects.AddOrUpdate(item);
            }
            context.SaveChanges();
        }

    }
}
