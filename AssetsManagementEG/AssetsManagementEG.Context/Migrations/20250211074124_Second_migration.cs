using Microsoft.EntityFrameworkCore.Migrations;

namespace AssetsManagementEG.Context.Migrations
{
    public partial class Second_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskCar_Tasks_TaskId",
                table: "TaskCar");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskEquipment_Tasks_TaskId",
                table: "TaskEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabors_Tasks_TaskId",
                table: "TaskLabors");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_District_DistrictId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Tassk");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_DistrictId",
                table: "Tassk",
                newName: "IX_Tassk_DistrictId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Labors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInService",
                table: "Labors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInService",
                table: "Equipment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tassk",
                table: "Tassk",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskCar_Tassk_TaskId",
                table: "TaskCar",
                column: "TaskId",
                principalTable: "Tassk",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskEquipment_Tassk_TaskId",
                table: "TaskEquipment",
                column: "TaskId",
                principalTable: "Tassk",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabors_Tassk_TaskId",
                table: "TaskLabors",
                column: "TaskId",
                principalTable: "Tassk",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tassk_District_DistrictId",
                table: "Tassk",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "DistrictId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskCar_Tassk_TaskId",
                table: "TaskCar");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskEquipment_Tassk_TaskId",
                table: "TaskEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabors_Tassk_TaskId",
                table: "TaskLabors");

            migrationBuilder.DropForeignKey(
                name: "FK_Tassk_District_DistrictId",
                table: "Tassk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tassk",
                table: "Tassk");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Labors");

            migrationBuilder.DropColumn(
                name: "IsInService",
                table: "Labors");

            migrationBuilder.DropColumn(
                name: "IsInService",
                table: "Equipment");

            migrationBuilder.RenameTable(
                name: "Tassk",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Tassk_DistrictId",
                table: "Tasks",
                newName: "IX_Tasks_DistrictId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskCar_Tasks_TaskId",
                table: "TaskCar",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskEquipment_Tasks_TaskId",
                table: "TaskEquipment",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabors_Tasks_TaskId",
                table: "TaskLabors",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_District_DistrictId",
                table: "Tasks",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "DistrictId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
