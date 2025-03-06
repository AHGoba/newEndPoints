using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class Adding_CompanyLabors_Relationship1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "CompanyLabors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CompanyLabors");
        }
    }
}
