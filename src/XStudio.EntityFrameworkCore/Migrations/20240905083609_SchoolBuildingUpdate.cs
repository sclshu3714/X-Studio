using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XStudio.Migrations
{
    /// <inheritdoc />
    public partial class SchoolBuildingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppX_SchoolBuildings",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppX_SchoolBuildings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppX_SchoolBuildings",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppX_SchoolBuildings",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppX_SchoolBuildings");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppX_SchoolBuildings");
        }
    }
}
