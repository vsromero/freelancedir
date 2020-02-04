using Microsoft.EntityFrameworkCore.Migrations;

namespace FreelanceDir.Data.Migrations
{
    public partial class removeStartingPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingPrice",
                table: "Gigs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "StartingPrice",
                table: "Gigs",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
