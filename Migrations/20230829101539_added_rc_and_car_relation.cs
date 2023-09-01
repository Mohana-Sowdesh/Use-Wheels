using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Use_Wheels.Migrations
{
    /// <inheritdoc />
    public partial class added_rc_and_car_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Vehicle_No",
                keyValue: "DL 89 JU 9921");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Vehicle_No",
                keyValue: "HR 82 KU 3214");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Vehicle_No",
                keyValue: "KL 14 FV 8845");

            migrationBuilder.DeleteData(
                table: "RC",
                keyColumn: "RC_No",
                keyValue: "635289");

            migrationBuilder.DeleteData(
                table: "RC",
                keyColumn: "RC_No",
                keyValue: "676725");

            migrationBuilder.DeleteData(
                table: "RC",
                keyColumn: "RC_No",
                keyValue: "788734");

            migrationBuilder.AddColumn<string>(
                name: "Vehicle_No",
                table: "RC",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RC_No",
                table: "Cars",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RC_No",
                table: "Cars",
                column: "RC_No");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_RC_RC_No",
                table: "Cars",
                column: "RC_No",
                principalTable: "RC",
                principalColumn: "RC_No");

            migrationBuilder.AddForeignKey(
                name: "FK_RC_Cars_RC_No",
                table: "RC",
                column: "RC_No",
                principalTable: "Cars",
                principalColumn: "Vehicle_No",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_RC_RC_No",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_RC_Cars_RC_No",
                table: "RC");

            migrationBuilder.DropIndex(
                name: "IX_Cars_RC_No",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Vehicle_No",
                table: "RC");

            migrationBuilder.DropColumn(
                name: "RC_No",
                table: "Cars");

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Vehicle_No", "Availability", "Category_Id", "Created_Date", "Description", "Img_URL", "Likes", "Pre_Owner_Count", "Price", "Updated_Date" },
                values: new object[,]
                {
                    { "DL 89 JU 9921", "available", 1, new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4020), "Some description", "D://car1.jpg", 0, 2, 2500000f, new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4020) },
                    { "HR 82 KU 3214", "available", 2, new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4030), "Some description", "D://car2.jpg", 0, 1, 3500000f, new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4030) },
                    { "KL 14 FV 8845", "available", 3, new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4030), "Some description", "D://car3.jpg", 0, 1, 1500000f, new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4030) }
                });

            migrationBuilder.InsertData(
                table: "RC",
                columns: new[] { "RC_No", "Board_Type", "Car_Model", "Colour", "Created_Date", "Date_Of_Reg", "FC_Validity", "Fuel_Type", "Insurance_Type", "Manufactured_Year", "Owner_Address", "Owner_Name", "Reg_Valid_Upto", "Updated_Date" },
                values: new object[,]
                {
                    { "635289", "Own board", "Honda CR-V", "Red", new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4040), new DateOnly(2001, 3, 1), new DateOnly(2025, 3, 1), "Diesel", "Third party", 2004, "Vasanth Vihar", "Ram", new DateOnly(2031, 3, 1), new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4050) },
                    { "676725", "Own board", "Honda Accord", "Crystal Black Pearl", new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4050), new DateOnly(2012, 7, 1), new DateOnly(2030, 3, 1), "Petrol", "Comprehensive", 2008, "Pathanamthitta", "Kaleel", new DateOnly(2033, 7, 1), new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4050) },
                    { "788734", "T board", "Volkswagen Golf", "Atlantic Blue Metallic", new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4050), new DateOnly(2003, 9, 1), new DateOnly(2027, 6, 1), "Petrol", "Zero Depreciation", 2011, "Gurgaon", "Shyam", new DateOnly(2033, 9, 1), new DateTime(2023, 8, 23, 21, 54, 35, 143, DateTimeKind.Local).AddTicks(4050) }
                });
        }
    }
}
