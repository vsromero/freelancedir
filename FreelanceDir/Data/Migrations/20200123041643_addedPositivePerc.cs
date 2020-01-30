using Microsoft.EntityFrameworkCore.Migrations;

namespace FreelanceDir.Data.Migrations
{
    public partial class addedPositivePerc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositivePercentage",
                table: "Gigs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PositivePercentage",
                table: "Gigs",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
