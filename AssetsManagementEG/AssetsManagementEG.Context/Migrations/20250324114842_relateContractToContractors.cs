using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class relateContractToContractors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarContractors_CarContractorsId",
                table: "Car");

            migrationBuilder.DropIndex(
                name: "IX_Car_CarContractorsId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarContractorsId",
                table: "Car");

            migrationBuilder.AddColumn<int>(
                name: "CarContractorsId",
                table: "Contract",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phoneNum",
                table: "CarContractors",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_CarContractorsId",
                table: "Contract",
                column: "CarContractorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_CarContractors_CarContractorsId",
                table: "Contract",
                column: "CarContractorsId",
                principalTable: "CarContractors",
                principalColumn: "CarContractorsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_CarContractors_CarContractorsId",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_CarContractorsId",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "CarContractorsId",
                table: "Contract");

            migrationBuilder.AlterColumn<int>(
                name: "phoneNum",
                table: "CarContractors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarContractorsId",
                table: "Car",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarContractorsId",
                table: "Car",
                column: "CarContractorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_CarContractors_CarContractorsId",
                table: "Car",
                column: "CarContractorsId",
                principalTable: "CarContractors",
                principalColumn: "CarContractorsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
