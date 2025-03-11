using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class removingIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DistrictCar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DistrictCar",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
