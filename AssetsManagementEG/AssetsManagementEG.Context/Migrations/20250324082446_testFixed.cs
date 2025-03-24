using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class testFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Contract",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CarContractorsId",
                table: "Car",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarContractors",
                columns: table => new
                {
                    CarContractorsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    phoneNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarContractors", x => x.CarContractorsId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_CarContractors_CarContractorsId",
                table: "Car");

            migrationBuilder.DropTable(
                name: "CarContractors");

            migrationBuilder.DropIndex(
                name: "IX_Car_CarContractorsId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "CarContractorsId",
                table: "Car");
        }
    }
}
