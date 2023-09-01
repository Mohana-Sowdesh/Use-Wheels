using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Use_Wheels.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_RC_RC_No",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_RC_Cars_RC_No",
                table: "RC");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "RC_No",
                keyValue: null,
                column: "RC_No",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RC_No",
                table: "Cars",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "RC",
                columns: new[] { "RC_No", "Board_Type", "Car_Model", "Colour", "Created_Date", "Date_Of_Reg", "FC_Validity", "Fuel_Type", "Insurance_Type", "Manufactured_Year", "Owner_Address", "Owner_Name", "Reg_Valid_Upto", "Updated_Date", "Vehicle_No" },
                values: new object[] { "635289", "Own board", "Honda CR-V", "Red", new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2130), new DateOnly(2001, 3, 1), new DateOnly(2025, 3, 1), "Diesel", "Third party", 2004, "Vasanth Vihar", "Ram", new DateOnly(2031, 3, 1), new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2130), "DL 89 JU 9921" });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Vehicle_No", "Availability", "Category_Id", "Created_Date", "Description", "Img_URL", "Likes", "Pre_Owner_Count", "Price", "RC_No", "Updated_Date" },
                values: new object[] { "DL 89 JU 9921", "available", 1, new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2150), "Some description", "D://car1.jpg", 0, 2, 2500000f, "635289", new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2150) });

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_RC_RC_No",
                table: "Cars",
                column: "RC_No",
                principalTable: "RC",
                principalColumn: "RC_No",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_RC_RC_No",
                table: "Cars");

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Vehicle_No",
                keyValue: "DL 89 JU 9921");

            migrationBuilder.DeleteData(
                table: "RC",
                keyColumn: "RC_No",
                keyValue: "635289");

            migrationBuilder.AlterColumn<string>(
                name: "RC_No",
                table: "Cars",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
