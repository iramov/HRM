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
            if (!context.Teams.Any())
            {
                SeedTeams(context);
            }
            if (!context.Employees.Any())
            {
                SeedEmployees(context);
            }
        }
        // Seed employees method
        private static void SeedEmployees(EmployeeDbContext context)
        {
            var employee = new List<Employee>
            {
                new Employee
                {
                    FirstName = "Hristo",
                    LastName = " Antov",
                    Position = Position.Trainee,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "f0ri@abv.bg",
                    CellNumber = "0999 99 55 33",
                    Address = {
                        Street = "Manastirski livadi",
                        City = "Sofia"
                        }
                },
                new Employee
                {
                    FirstName = "Iliqn",
                    LastName = " Ramov",
                    Position = Position.Trainee,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "iliqn@abv.bg",
                    CellNumber = "0999 11 22 33",
                    Address = {
                        Street = "Center",
                        City = "Sofia"
                        }
                },
                new Employee
                {
                    FirstName = "Damqn",
                    LastName = " Grancharov",
                    Position = Position.Trainee,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "damqn@abv.bg",
                    CellNumber = "0999 99 88 77",
                    Address = {
                        Street = "Center",
                        City = "Sofia"
                        }
                },
                new Employee
                {
                    FirstName = "Boris",
                    LastName = " Borovski",
                    Position = Position.Trainee,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "boris@abv.bg",
                    CellNumber = "0999 70 85 12",
                    Address = {
                        Street = "Center",
                        City = "Sofia"
                        }},
                new Employee
                {
                    FirstName = "Mitko",
                    LastName = " Ivanov",
                    Position = Position.Trainee,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "mitko@abv.bg",
                    CellNumber = "0999 11 22 33",
                    Address = {
                        Street = "Center",
                        City = "Sofia"
                        }
                },
                new Employee
                {
                    FirstName = "Juliyan",
                    LastName = " Boyanov",
                    Position = Position.TeamLeader,
                    Salary = 10000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "juliqn@gmail.bg",
                    Address = {
                        Street = "Center",
                        City = "Sofia"
                        }
                },
                new Employee
                {
                    FirstName = "Desislav",
                    LastName = " Bonchev",
                    Position = Position.TeamLeader,
                    Salary = 2000,
                    Workplace = "SoftServe Bulgaria",
                    Email = "desislav@abv.bg",
                    CellNumber = "0979 423 432",
                    Address = {
                        Street = "Center",
                        City = "Sofia"
                        }
                }
                //,
                //new Employee
                //{
                //    FirstName = "Iliqn",
                //    LastName = " Ramov",
                //    Position = Position.Trainee,
                //    Salary = 2000,
                //    Workplace = "SoftServe Bulgaria",
                //    Email = "iliqn@abv.bg",
                //    CellNumber = "0999 11 22 33",
                //    Address = {
                //        Street = "Center",
                //        City = "Sofia"
                //        }

                //    Name = "Vladimir",
                //    Position = Position.Junior,
                //    Salary = 10000,
                //    Workplace = "SoftServe Ukraine",
                //    Email = "vladimir@gmail.com",
                //    Address = "Ukraine",
                //    ProjectId = 2,
                //    TeamId = 3
                //},
                //new Employee
                //{
                //    FirstName = "Iliqn",
                //    LastName = " Ramov",
                //    Position = Position.Trainee,
                //    Salary = 2000,
                //    Workplace = "SoftServe Bulgaria",
                //    Email = "iliqn@abv.bg",
                //    CellNumber = "0999 11 22 33",
                //    Address = {
                //        Street = "Center",
                //        City = "Sofia"
                //        }

                //    Name = "Vitalii",
                //    Position = Position.Senior,
                //    Salary = 10000,
                //    Workplace = "SoftServe Ukraine",
                //    Email = "vitalii@gmail.com",
                //    Address = "Ukraine",
                //    ProjectId = 2,
                //    TeamId = 3
                //},
                //new Employee
                //{
                //    FirstName = "Iliqn",
                //    LastName = " Ramov",
                //    Position = Position.Trainee,
                //    Salary = 2000,
                //    Workplace = "SoftServe Bulgaria",
                //    Email = "iliqn@abv.bg",
                //    CellNumber = "0999 11 22 33",
                //    Address = {
                //        Street = "Center",
                //        City = "Sofia"
                //        }

                //    Name = "John Johnson",
                //    Position = Position.ProjectManager,
                //    Salary = 15000,
                //    Workplace = "SoftServe Ukraine",
                //    Email = "john@gmail.com",
                //    Address = "Ukraine",
                //    ProjectId = 1,
                //    TeamId = 1
                //},
                //new Employee
                //{
                //    FirstName = "Iliqn",
                //    LastName = " Ramov",
                //    Position = Position.Trainee,
                //    Salary = 2000,
                //    Workplace = "SoftServe Bulgaria",
                //    Email = "iliqn@abv.bg",
                //    CellNumber = "0999 11 22 33",
                //    Address = {
                //        Street = "Center",
                //        City = "Sofia"
                //        }

                //    Name = "Christopher Baker",
                //    Position = Position.ProjectManager,
                //    Salary = 15000,
                //    Workplace = "SoftServe Ukraine",
                //    Email = "christopher@gmail.com",
                //    Address = "Ukraine",
                //    ProjectId = 2,
                //    TeamId = 3
                    
                //},
                //new Employee
                //{
                //    FirstName = "Iliqn",
                //    LastName = " Ramov",
                //    Position = Position.Trainee,
                //    Salary = 2000,
                //    Workplace = "SoftServe Bulgaria",
                //    Email = "iliqn@abv.bg",
                //    CellNumber = "0999 11 22 33",
                //    Address = {
                //        Street = "Center",
                //        City = "Sofia"
                //        }

                //    Name = "Andriy Stytsyuk",
                //    Position = Position.DeliveryDirector,
                //    Salary = 15000,
                //    Workplace = "SoftServe Ukraine",
                //    Email = "adriy@gmail.com",
                //    Address = "Ukraine"
                //},
                //new Employee
                //{
                //    FirstName = "Iliqn",
                //    LastName = " Ramov",
                //    Position = Position.Trainee,
                //    Salary = 2000,
                //    Workplace = "SoftServe Bulgaria",
                //    Email = "iliqn@abv.bg",
                //    CellNumber = "0999 11 22 33",
                //    Address = {
                //        Street = "Center",
                //        City = "Sofia"
                //        }

                //    Name = "Oleh Denys",
                //    Position = Position.CEO,
                //    Salary = 20000,
                //    Workplace = "SoftServe Ukraine",
                //    Email = "oleh@gmail.com",
                //    Address = "Ukraine",                    
                //
        };

            foreach (var item in employee)
            {
                context.Employees.AddOrUpdate(item);
            }
            context.SaveChanges();
        }

        // Seed teams method
        private static void SeedTeams(EmployeeDbContext context)
        {
            var teams = new List<Team>{
              new Team
              {
                  LeaderId = 6,
                  Name = "HR system Back End",
                  ProjectId = 1,
                  Delivery = DeliveryUnit.Entertainment
              },
              new Team
              {
                  LeaderId = 7,
                  Name = "HR system Front End",
                  ProjectId = 1,
                  Delivery = DeliveryUnit.Entertainment
              },
              new Team
              {
                  LeaderId = 6, 
                  Name = "NASA system",
                  ProjectId = 2,
                  Delivery = DeliveryUnit.Entertainment
              }};

            foreach (var item in teams)
            {
                context.Teams.AddOrUpdate(item);
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
                      Delivery = DeliveryUnit.Entertainment
                      //Description = "System that helps Hr's to manage employees in company"
                  },
                  new Project
                  {
                      Name = "New NASA operating system",
                      Delivery = DeliveryUnit.Finance
                      //Description = "It's top secret"
                  }};

            foreach (var item in projects)
            {
                context.Projects.AddOrUpdate(item);
            }
            context.SaveChanges();
        }
         
    }
}
