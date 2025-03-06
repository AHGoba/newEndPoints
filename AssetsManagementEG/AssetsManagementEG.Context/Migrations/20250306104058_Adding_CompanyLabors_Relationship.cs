using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class Adding_CompanyLabors_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyL",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyL", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyLabors",
                columns: table => new
                {
                    ComapanyID = table.Column<int>(nullable: false),
                    LaborsID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLabors", x => new { x.LaborsID, x.ComapanyID });
                    table.ForeignKey(
                        name: "FK_CompanyLabors_CompanyL_ComapanyID",
                        column: x => x.ComapanyID,
                        principalTable: "CompanyL",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyLabors_Labors_LaborsID",
                        column: x => x.LaborsID,
                        principalTable: "Labors",
                        principalColumn: "LaborsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLabors_ComapanyID",
                table: "CompanyLabors",
                column: "ComapanyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyLabors");

            migrationBuilder.DropTable(
                name: "CompanyL");
        }
    }
}
