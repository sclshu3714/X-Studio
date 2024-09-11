using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XStudio.Migrations
{
    /// <inheritdoc />
    public partial class hasIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_AppX_Sections_ScheduleCode",
                table: "AppX_Sections",
                newName: "IX_TimePeriod_ScheduleCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_SchoolCampuses_SchoolCode",
                table: "AppX_SchoolCampuses",
                newName: "IX_TimePeriod_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_SchoolBuildings_SchoolCode",
                table: "AppX_SchoolBuildings",
                newName: "IX_TimePeriod_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_SchoolBuildings_SchoolCampusCode",
                table: "AppX_SchoolBuildings",
                newName: "IX_TimePeriod_SchoolCampusCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_Classrooms_SchoolCode",
                table: "AppX_Classrooms",
                newName: "IX_TimePeriod_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_Classrooms_SchoolCampusCode",
                table: "AppX_Classrooms",
                newName: "IX_TimePeriod_SchoolCampusCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_Classrooms_FloorCode",
                table: "AppX_Classrooms",
                newName: "IX_TimePeriod_FloorCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_Classrooms_BuildingCode",
                table: "AppX_Classrooms",
                newName: "IX_TimePeriod_BuildingCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_BuildingFloors_SchoolCode",
                table: "AppX_BuildingFloors",
                newName: "IX_TimePeriod_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_BuildingFloors_SchoolCampusCode",
                table: "AppX_BuildingFloors",
                newName: "IX_TimePeriod_SchoolCampusCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppX_BuildingFloors_BuildingCode",
                table: "AppX_BuildingFloors",
                newName: "IX_TimePeriod_BuildingCode");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_TimePeriods",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_TimePeriods",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Sections",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Sections",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Schools",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Schools",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_SchoolCampuses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_SchoolCampuses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_SchoolBuildings",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_SchoolBuildings",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Schedules",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Schedules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Courses",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Courses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Classrooms",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Classrooms",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_BuildingFloors",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_BuildingFloors",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_TimePeriods");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_TimePeriods");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Sections");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Sections");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Schools");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Schools");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_SchoolCampuses");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_SchoolCampuses");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Schedules");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Schedules");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Courses");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Courses");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Code",
                table: "AppX_BuildingFloors");

            migrationBuilder.DropIndex(
                name: "IX_TimePeriod_Name",
                table: "AppX_BuildingFloors");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_ScheduleCode",
                table: "AppX_Sections",
                newName: "IX_AppX_Sections_ScheduleCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCode",
                table: "AppX_SchoolCampuses",
                newName: "IX_AppX_SchoolCampuses_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCode",
                table: "AppX_SchoolBuildings",
                newName: "IX_AppX_SchoolBuildings_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCampusCode",
                table: "AppX_SchoolBuildings",
                newName: "IX_AppX_SchoolBuildings_SchoolCampusCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCode",
                table: "AppX_Classrooms",
                newName: "IX_AppX_Classrooms_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCampusCode",
                table: "AppX_Classrooms",
                newName: "IX_AppX_Classrooms_SchoolCampusCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_FloorCode",
                table: "AppX_Classrooms",
                newName: "IX_AppX_Classrooms_FloorCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_BuildingCode",
                table: "AppX_Classrooms",
                newName: "IX_AppX_Classrooms_BuildingCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCode",
                table: "AppX_BuildingFloors",
                newName: "IX_AppX_BuildingFloors_SchoolCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_SchoolCampusCode",
                table: "AppX_BuildingFloors",
                newName: "IX_AppX_BuildingFloors_SchoolCampusCode");

            migrationBuilder.RenameIndex(
                name: "IX_TimePeriod_BuildingCode",
                table: "AppX_BuildingFloors",
                newName: "IX_AppX_BuildingFloors_BuildingCode");
        }
    }
}
