using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Use_Wheels.Migrations
{
    /// <inheritdoc />
    public partial class orderTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User_ID",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Vehicle_No",
                keyValue: "DL 89 JU 9921",
                columns: new[] { "Created_Date", "Updated_Date" },
                values: new object[] { new DateTime(2023, 8, 31, 19, 26, 25, 102, DateTimeKind.Local).AddTicks(9970), new DateTime(2023, 8, 31, 19, 26, 25, 102, DateTimeKind.Local).AddTicks(9970) });

            migrationBuilder.UpdateData(
                table: "RC",
                keyColumn: "RC_No",
                keyValue: "635289",
                columns: new[] { "Created_Date", "Updated_Date" },
                values: new object[] { new DateTime(2023, 8, 31, 19, 26, 25, 102, DateTimeKind.Local).AddTicks(9950), new DateTime(2023, 8, 31, 19, 26, 25, 102, DateTimeKind.Local).AddTicks(9950) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "User_ID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Vehicle_No",
                keyValue: "DL 89 JU 9921",
                columns: new[] { "Created_Date", "Updated_Date" },
                values: new object[] { new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2150), new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2150) });

            migrationBuilder.UpdateData(
                table: "RC",
                keyColumn: "RC_No",
                keyValue: "635289",
                columns: new[] { "Created_Date", "Updated_Date" },
                values: new object[] { new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2130), new DateTime(2023, 8, 29, 16, 14, 49, 437, DateTimeKind.Local).AddTicks(2130) });
        }
    }
}
