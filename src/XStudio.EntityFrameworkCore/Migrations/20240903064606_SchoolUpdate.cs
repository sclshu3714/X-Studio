using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XStudio.Migrations
{
    /// <inheritdoc />
    public partial class SchoolUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppXSchoolBuildings",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppXClassrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppXClassrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingCode",
                table: "AppXClassrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppXBuildingFloors",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppXBuildingFloors",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppXSchoolBuildings_SchoolCode",
                table: "AppXSchoolBuildings",
                column: "SchoolCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppXClassrooms_BuildingCode",
                table: "AppXClassrooms",
                column: "BuildingCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppXClassrooms_SchoolCampusCode",
                table: "AppXClassrooms",
                column: "SchoolCampusCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppXClassrooms_SchoolCode",
                table: "AppXClassrooms",
                column: "SchoolCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppXBuildingFloors_SchoolCampusCode",
                table: "AppXBuildingFloors",
                column: "SchoolCampusCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppXBuildingFloors_SchoolCode",
                table: "AppXBuildingFloors",
                column: "SchoolCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AppXBuildingFloors_AppXSchoolCampuses_SchoolCampusCode",
                table: "AppXBuildingFloors",
                column: "SchoolCampusCode",
                principalTable: "AppXSchoolCampuses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppXBuildingFloors_AppXSchools_SchoolCode",
                table: "AppXBuildingFloors",
                column: "SchoolCode",
                principalTable: "AppXSchools",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppXClassrooms_AppXSchoolBuildings_BuildingCode",
                table: "AppXClassrooms",
                column: "BuildingCode",
                principalTable: "AppXSchoolBuildings",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppXClassrooms_AppXSchoolCampuses_SchoolCampusCode",
                table: "AppXClassrooms",
                column: "SchoolCampusCode",
                principalTable: "AppXSchoolCampuses",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppXClassrooms_AppXSchools_SchoolCode",
                table: "AppXClassrooms",
                column: "SchoolCode",
                principalTable: "AppXSchools",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppXSchoolBuildings_AppXSchools_SchoolCode",
                table: "AppXSchoolBuildings",
                column: "SchoolCode",
                principalTable: "AppXSchools",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppXBuildingFloors_AppXSchoolCampuses_SchoolCampusCode",
                table: "AppXBuildingFloors");

            migrationBuilder.DropForeignKey(
                name: "FK_AppXBuildingFloors_AppXSchools_SchoolCode",
                table: "AppXBuildingFloors");

            migrationBuilder.DropForeignKey(
                name: "FK_AppXClassrooms_AppXSchoolBuildings_BuildingCode",
                table: "AppXClassrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_AppXClassrooms_AppXSchoolCampuses_SchoolCampusCode",
                table: "AppXClassrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_AppXClassrooms_AppXSchools_SchoolCode",
                table: "AppXClassrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_AppXSchoolBuildings_AppXSchools_SchoolCode",
                table: "AppXSchoolBuildings");

            migrationBuilder.DropIndex(
                name: "IX_AppXSchoolBuildings_SchoolCode",
                table: "AppXSchoolBuildings");

            migrationBuilder.DropIndex(
                name: "IX_AppXClassrooms_BuildingCode",
                table: "AppXClassrooms");

            migrationBuilder.DropIndex(
                name: "IX_AppXClassrooms_SchoolCampusCode",
                table: "AppXClassrooms");

            migrationBuilder.DropIndex(
                name: "IX_AppXClassrooms_SchoolCode",
                table: "AppXClassrooms");

            migrationBuilder.DropIndex(
                name: "IX_AppXBuildingFloors_SchoolCampusCode",
                table: "AppXBuildingFloors");

            migrationBuilder.DropIndex(
                name: "IX_AppXBuildingFloors_SchoolCode",
                table: "AppXBuildingFloors");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppXSchoolBuildings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppXClassrooms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppXClassrooms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingCode",
                table: "AppXClassrooms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppXBuildingFloors",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppXBuildingFloors",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
