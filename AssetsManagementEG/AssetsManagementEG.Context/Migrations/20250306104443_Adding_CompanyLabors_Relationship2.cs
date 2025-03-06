using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class Adding_CompanyLabors_Relationship2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Labors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Labors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
