using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffTracking.Data.Migrations
{
    public partial class AddEmployeeCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "access_card",
                table: "employees",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "access_card",
                table: "employees");
        }
    }
}
