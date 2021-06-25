using Microsoft.EntityFrameworkCore.Migrations;

namespace forum.business.Migrations
{
    public partial class FileScan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScanResult",
                table: "Discussions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScanResult",
                table: "Discussions");
        }
    }
}
