using System;
using System.Drawing;
using System.Reflection.Metadata;
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
                    Updated_Date = DateTime.Now,
                    RC_No = "635289"
                }
            );
        }
    }
}

