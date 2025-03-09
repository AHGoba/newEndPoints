using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class MMContractCarv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractName = table.Column<string>(nullable: true),
                    ContractType = table.Column<string>(nullable: true),
                    ContractDescreption = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DistrictId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contract_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractsCars",
                columns: table => new
                {
                    ContractId = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractsCars", x => new { x.CarId, x.ContractId });
                    table.ForeignKey(
                        name: "FK_ContractsCars_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractsCars_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_DistrictId",
                table: "Contract",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsCars_ContractId",
                table: "ContractsCars",
                column: "ContractId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractsCars");

            migrationBuilder.DropTable(
                name: "Contract");
        }
    }
}
