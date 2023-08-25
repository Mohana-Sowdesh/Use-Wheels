using System;
using System.Drawing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Use_Wheels.Models.DTO;

namespace Use_Wheels.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserDTO>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserDTO> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<RC> RC { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<UserDTO>().HasData(
            //    new UserDTO
            //    {
            //        Id = "1",
            //        UserName = "admin1",
            //        PasswordHash = "Admin@1",
            //        Role = "admin",
            //        Email = "admin1@cars24.com",
            //        First_Name = "Admin",
            //        Last_Name = "Admin",
            //        Dob = new DateOnly(1980, 3, 1),
            //        Phone_Number = "8343949349",
            //        Gender = "Male"
            //    }
            //);

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Category_Id = 1,
                    Category_Names = "SUV"
                },

                new Category
                {
                    Category_Id = 2,
                    Category_Names = "Hatchback"
                },

                new Category
                {
                    Category_Id = 3,
                    Category_Names = "Sedan"
                }
            );

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Vehicle_No = "DL 89 JU 9921",
                    Category_Id = 1,
                    Description = "Some description",
                    Pre_Owner_Count = 2,
                    Img_URL = "D://car1.jpg",
                    Price = 2500000,
                    Created_Date = DateTime.Now,
                    Updated_Date = DateTime.Now
                },
                new Car
                {
                    Vehicle_No = "HR 82 KU 3214",
                    Category_Id = 2,
                    Description = "Some description",
                    Pre_Owner_Count = 1,
                    Img_URL = "D://car2.jpg",
                    Price = 3500000,
                    Created_Date = DateTime.Now,
                    Updated_Date = DateTime.Now
                },
                new Car
                {
                    Vehicle_No = "KL 14 FV 8845",
                    Category_Id = 3,
                    Description = "Some description",
                    Pre_Owner_Count = 1,
                    Img_URL = "D://car3.jpg",
                    Price = 1500000,
                    Created_Date = DateTime.Now,
                    Updated_Date = DateTime.Now
                }
            );

            modelBuilder.Entity<RC>().HasData(
                new RC
                {
                    RC_No = "635289",
                    Vehicle_No = "DL 89 JU 9921",
                    Date_Of_Reg = new DateOnly(2001, 3, 1),
                    Reg_Valid_Upto = new DateOnly(2031, 3, 1),
                    Owner_Name = "Ram",
                    Owner_Address = "Vasanth Vihar",
                    Board_Type = "Own board",
                    FC_Validity = new DateOnly(2025, 3, 1),
                    Insurance_Type = "Third party",
                    Car_Model = "Honda CR-V",
                    Manufactured_Year = 2004,
                    Fuel_Type = "Diesel",
                    Colour = "Red",
                    Created_Date = DateTime.Now,
                    Updated_Date = DateTime.Now
                },
                new RC
                {
                    RC_No = "788734",
                    Vehicle_No = "HR 82 KU 3214",
                    Date_Of_Reg = new DateOnly(2003, 9, 1),
                    Reg_Valid_Upto = new DateOnly(2033, 9, 1),
                    Owner_Name = "Shyam",
                    Owner_Address = "Gurgaon",
                    Board_Type = "T board",
                    FC_Validity = new DateOnly(2027, 6, 1),
                    Insurance_Type = "Zero Depreciation",
                    Car_Model = "Volkswagen Golf",
                    Manufactured_Year = 2011,
                    Fuel_Type = "Petrol",
                    Colour = "Atlantic Blue Metallic",
                    Created_Date = DateTime.Now,
                    Updated_Date = DateTime.Now
                },
                new RC
                {
                    RC_No = "676725",
                    Vehicle_No = "KL 14 FV 8845",
                    Date_Of_Reg = new DateOnly(2012, 7, 1),
                    Reg_Valid_Upto = new DateOnly(2033, 7, 1),
                    Owner_Name = "Kaleel",
                    Owner_Address = "Pathanamthitta",
                    Board_Type = "Own board",
                    FC_Validity = new DateOnly(2030, 3, 1),
                    Insurance_Type = "Comprehensive",
                    Car_Model = "Honda Accord",
                    Manufactured_Year = 2008,
                    Fuel_Type = "Petrol",
                    Colour = "Crystal Black Pearl",
                    Created_Date = DateTime.Now,
                    Updated_Date = DateTime.Now
                }
            );
        }
    }
}

