using Microsoft.EntityFrameworkCore.Migrations;

namespace SanctionScanner.Migrations
{
    public partial class AddRefrenceID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefrenceId",
                table: "Sanctions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefrenceId",
                table: "Sanctions");
        }
    }
}
