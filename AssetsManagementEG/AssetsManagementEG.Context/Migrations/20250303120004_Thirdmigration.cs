using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class Thirdmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Tassk",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Labors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlateNum2",
                table: "Car",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Tassk");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Labors");

            migrationBuilder.DropColumn(
                name: "PlateNum2",
                table: "Car");
        }
    }
}
