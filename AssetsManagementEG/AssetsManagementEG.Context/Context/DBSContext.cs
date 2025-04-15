using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Models.Models.Archive;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Context.Context
{
   public class DBSContext : IdentityDbContext<ApplicationUser>
    {
            public DBSContext(DbContextOptions<DBSContext> options) : base(options)
            {
            }
            public DBSContext() 
            {
            
            }
            public DbSet<Car> Car { get; set; }
            public DbSet<Tassk> Tassk { get; set; }
            public DbSet<District> District { get; set; }
            public DbSet<Labors> Labors { get; set; }
            public DbSet<Equipment> Equipment { get; set; }
            public DbSet<TaskCar> TaskCar { get; set; }
            public DbSet<TaskEquipment> TaskEquipment { get; set; }
            public DbSet<TaskLabors> TaskLabors { get; set; }
            public DbSet<DistrictCar> DistrictCar { get; set; }
            public DbSet<DistrictEquibment> DistrictEquibment { get; set; }
            public DbSet<DistrictLabors> DistrictLabors { get; set; }
            public DbSet<UsersDistrict> UsersDistrict { get; set; }
            public DbSet<CarContractors>   CarContractors { get; set; }
            

            // company with labors 
            public DbSet<CompanyL> CompanyL { get; set; }
            public DbSet<CompanyLabors> CompanyLabors { get; set; }

            // Contracts with cars 
            public DbSet<Contract> Contract { get; set; }
            public DbSet<ContractsCars> ContractsCars { get; set; }

           // Archives
            public DbSet<CarArchieve> CarArchive { get; set; }
            public DbSet<EquipmentArchieve> equipmentArchieves { get; set; }
            public DbSet<LaborArchieve> laborArchieves { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);


                //111111111111111111
                // Configure the many-to-many relationship for TaskCar
                modelBuilder.Entity<TaskCar>().HasKey(tc => new { tc.TaskId, tc.CarId });

                // Configure the relationship between TaskCar and Task
                modelBuilder.Entity<TaskCar>()
                .HasOne(tc => tc.Car) // TaskCar has one Car
                .WithMany(c => c.TaskCars) // Car has many TaskCars
                .HasForeignKey(tc => tc.CarId); // Foreign key in TaskCar

                // Configure the relationship between TaskCar and Car
                modelBuilder.Entity<TaskCar>()
                .HasOne(tc => tc.Task)
                .WithMany(c => c.TaskCars)
                .HasForeignKey(tc => tc.TaskId);

                //222222222222222222222
                // Configure the many-to-many relationship for TaskEquipment
                modelBuilder.Entity<TaskEquipment>().HasKey(tc => new { tc.TaskId, tc.EquipmentId });
                // Configure the relationship between TaskEqui and Equipment
                modelBuilder.Entity<TaskEquipment>()
                .HasOne(tc => tc.Equipment)
                .WithMany(c => c.TaskEquipment)
                .HasForeignKey(tc => tc.EquipmentId);


                modelBuilder.Entity<TaskEquipment>()
               .HasOne(tc => tc.Task)
               .WithMany(c => c.TaskEquipment)
               .HasForeignKey(tc => tc.TaskId);

                //3333333333333333333333333
                // Configure the many-to-many relationship for TaskLabors
                modelBuilder.Entity<TaskLabors>().HasKey(tc => new { tc.TaskId, tc.LaborsId });

                // Configure the relationship between TaskLabors and Labor
                modelBuilder.Entity<TaskLabors>()
               .HasOne(tc => tc.Labors)
               .WithMany(c => c.TaskLabors)
               .HasForeignKey(tc => tc.LaborsId);

                // Configure the relationship between TaskLabors and Task
                modelBuilder.Entity<TaskLabors>()
               .HasOne(tc => tc.Task)
               .WithMany(c => c.TaskLabors)
               .HasForeignKey(tc => tc.TaskId);

                //444444444444444444444444444
                // Configure the many-to-many relationship for DistrictCar
                modelBuilder.Entity<DistrictCar>().HasKey(tc => new { tc.DistrictId, tc.CarId });

                // Configure the relationship between DistrictCar and car
                modelBuilder.Entity<DistrictCar>()
               .HasOne(tc => tc.Car)
               .WithMany(c => c.DistrictCar)
               .HasForeignKey(tc => tc.CarId);

                // Configure the relationship between DistrictCar and district
                modelBuilder.Entity<DistrictCar>()
               .HasOne(tc => tc.District)
               .WithMany(c => c.DistrictCar)
               .HasForeignKey(tc => tc.DistrictId);

                //5555555555555555555555555555
                // Configure the many-to-many relationship for DistrictEqui
                modelBuilder.Entity<DistrictEquibment>().HasKey(tc => new { tc.DistrictId, tc.EquipmentId });

                // Configure the relationship between DistrictEqui and Equi
                modelBuilder.Entity<DistrictEquibment>()
               .HasOne(tc => tc.Equipment)
               .WithMany(c => c.DistrictEquibment)
               .HasForeignKey(tc => tc.EquipmentId);

                // Configure the relationship between DistrictEqui and Distr
                modelBuilder.Entity<DistrictEquibment>()
               .HasOne(tc => tc.District)
               .WithMany(c => c.DistrictEquibment)
               .HasForeignKey(tc => tc.DistrictId);

                //666666666666666666666666666666
                // Configure the many-to-many relationship for DistrictLabors
                modelBuilder.Entity<DistrictLabors>().HasKey(tc => new { tc.DistrictId, tc.LaborsId });

                // Configure the relationship between DistrictLabors and District
                modelBuilder.Entity<DistrictLabors>()
               .HasOne(tc => tc.District)
               .WithMany(c => c.DistrictLabors)
               .HasForeignKey(tc => tc.DistrictId);

                // Configure the relationship between DistrictLabors and labors
                modelBuilder.Entity<DistrictLabors>()
               .HasOne(tc => tc.Labors)
               .WithMany(c => c.DistrictLabors)
               .HasForeignKey(tc => tc.LaborsId);

                //77777777777777777777777777777777
                // Configure the many-to-many relationship for UsersDistrict
                modelBuilder.Entity<UsersDistrict>().HasKey(tc => new { tc.DistrictId, tc.ApplicationUserId });

                // Configure the relationship between UsersDistrict and District
                modelBuilder.Entity<UsersDistrict>()
               .HasOne(tc => tc.District)
               .WithMany(c => c.UsersDistrict)
               .HasForeignKey(tc => tc.DistrictId);

                // Configure the relationship between UsersDistrict and Users
                modelBuilder.Entity<UsersDistrict>()
               .HasOne(tc => tc.applicationUser)
               .WithMany(c => c.UsersDistricts)
               .HasForeignKey(tc => tc.ApplicationUserId);

                //888888888888888888888888888888888
                // Configure the many-to-many relationship for CompanyLabors
                modelBuilder.Entity<CompanyLabors>().HasKey(cl => new { cl.LaborsID, cl.ComapanyID });

                // Configure the relationship between CompanyLabors and Labors
                modelBuilder.Entity<CompanyLabors>()
                .HasOne(cl => cl.Labors)
                .WithMany(l=> l.CompanyLabors)
                .HasForeignKey(cl=> cl.LaborsID);

                // Configure the relationship between CompanyLabors and Company
                modelBuilder.Entity<CompanyLabors>()
                .HasOne(cl=> cl.CompanyL)
                .WithMany(c=> c.CompanyLabors)
                .HasForeignKey(cl=> cl.ComapanyID);

                //99999999999999999999999999999999999999
                // Configure the many-to-many relationship for ContractsCars
                modelBuilder.Entity<ContractsCars>().HasKey(cc => new {cc.CarId , cc.ContractId });

                // Configure the relationship between ContractsCars and Cars
                 modelBuilder.Entity<ContractsCars>()
                .HasOne(cc=> cc.Car)
                .WithMany(c=>c.ContractsCars)
                .HasForeignKey(cc=> cc.CarId);

                // Configure the relationship between ContractsCars and Contract
                modelBuilder.Entity<ContractsCars>()
                .HasOne(cc=> cc.Contract)
                .WithMany(c=>c.ContractsCars)
                .HasForeignKey(cc=> cc.ContractId);



        }
    }
}
