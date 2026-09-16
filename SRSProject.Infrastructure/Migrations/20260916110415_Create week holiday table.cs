using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SRSProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Createweekholidaytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekHolidayId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WeeklyHolidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOfHoliday = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaysOfHoliday = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyHolidays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WeekHolidayId",
                table: "Employees",
                column: "WeekHolidayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_WeeklyHolidays_WeekHolidayId",
                table: "Employees",
                column: "WeekHolidayId",
                principalTable: "WeeklyHolidays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_WeeklyHolidays_WeekHolidayId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "WeeklyHolidays");

            migrationBuilder.DropIndex(
                name: "IX_Employees_WeekHolidayId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WeekHolidayId",
                table: "Employees");
        }
    }
}
