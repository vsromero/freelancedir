using Microsoft.EntityFrameworkCore.Migrations;

namespace FreelanceDir.Data.Migrations
{
    public partial class addImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Gigs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Gigs");
        }
    }
}
