using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace XStudio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorizationId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicationId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_TimePeriods",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_TimePeriods",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_TimePeriods",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_TimePeriods",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_TimePeriods",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_TimePeriods",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_TimePeriods",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Sections",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "AppX_Sections",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true,
                comment: "节次开始时间");

            migrationBuilder.AlterColumn<string>(
                name: "ScheduleCode",
                table: "AppX_Sections",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "节次编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PeriodCode",
                table: "AppX_Sections",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "时段编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_Sections",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Sections",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_Sections",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTeaching",
                table: "AppX_Sections",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                comment: "是否是授课");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelfStudy",
                table: "AppX_Sections",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                comment: "是否是自习");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanner",
                table: "AppX_Sections",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                comment: "是否是通栏");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_Sections",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "AppX_Sections",
                type: "time(6)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true,
                comment: "节次结束时间");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_Sections",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Sections",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Schools",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<string>(
                name: "PromotionSlogan",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                comment: "宣传语")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Schools",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_Schools",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_Schools",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                comment: "简介")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_Schools",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Badge",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                comment: "校徽")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                comment: "校址")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Schools",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_SchoolCampuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_SchoolCampuses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_SchoolCampuses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_SchoolCampuses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_SchoolCampuses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_SchoolCampuses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AppX_SchoolCampuses",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                comment: "校址")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_SchoolCampuses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_SchoolBuildings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_SchoolBuildings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_SchoolBuildings",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_SchoolBuildings",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Schedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_Schedules",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Schedules",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "节次方案名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LayoutOfWeek",
                table: "AppX_Schedules",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                comment: "布局节次表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_Schedules",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_Schedules",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_Schedules",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Schedules",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "AppX_Schedules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "AppX_Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SchoolYear",
                table: "AppX_Schedules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "AppX_Schedules",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_RoomUsages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_RoomUsages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_RoomUsages",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_RoomUsages",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_RoomUsages",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_RoomUsages",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_RoomUsages",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Projects",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "项目名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorId",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_Courses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Courses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "课程名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_Courses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_Courses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_Courses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Courses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Classrooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<string>(
                name: "UsageCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "用途编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "学校编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "校区编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_Classrooms",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_Classrooms",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_Classrooms",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FloorCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "楼层编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_Classrooms",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "楼栋编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_BuildingFloors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                comment: "数据有效标识");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "学校编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "校区编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_BuildingFloors",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                comment: "序号");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AppX_BuildingFloors",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AppX_BuildingFloors",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AppX_BuildingFloors",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingCode",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                comment: "楼栋编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128,
                comment: "编码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUserTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpUserTokens",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUserRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AbpUserRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpUserRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUserLogins",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpUserLogins",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetUserId",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "SourceUserId",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpUserClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpUserClaims",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpUserClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpTenants",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpTenantConnectionStrings",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpSettings",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpSettingDefinitions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpSessions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpSessions",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpSessions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpSecurityLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpSecurityLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpSecurityLogs",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpRoleClaims",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AbpRoleClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpRoleClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpPermissions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpPermissionGroups",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpPermissionGrants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpPermissionGrants",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifierId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeleterId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetUserId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetTenantId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "SourceUserId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "SourceTenantId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpFeatureValues",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpFeatures",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpFeatureGroups",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpEntityPropertyChanges",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityChangeId",
                table: "AbpEntityPropertyChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpEntityPropertyChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "EntityTenantId",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuditLogId",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpClaimTypes",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpBackgroundJobs",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImpersonatorUserId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImpersonatorTenantId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AbpAuditLogActions",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuditLogId",
                table: "AbpAuditLogActions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AbpAuditLogActions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(36)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "AppX_Schedules");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "AppX_Schedules");

            migrationBuilder.DropColumn(
                name: "SchoolYear",
                table: "AppX_Schedules");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "AppX_Schedules");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorizationId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "OpenIddictTokens",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "OpenIddictScopes",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationId",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "OpenIddictAuthorizations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "OpenIddictApplications",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_TimePeriods",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_TimePeriods",
                type: "int",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_TimePeriods",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_TimePeriods",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_TimePeriods",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_TimePeriods",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_TimePeriods",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Sections",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "AppX_Sections",
                type: "time(6)",
                nullable: true,
                comment: "节次开始时间",
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ScheduleCode",
                table: "AppX_Sections",
                type: "varchar(128)",
                nullable: false,
                comment: "节次编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PeriodCode",
                table: "AppX_Sections",
                type: "varchar(128)",
                nullable: false,
                comment: "时段编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_Sections",
                type: "int",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Sections",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_Sections",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsTeaching",
                table: "AppX_Sections",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否是授课",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelfStudy",
                table: "AppX_Sections",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否是自习",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanner",
                table: "AppX_Sections",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否是通栏",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_Sections",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "AppX_Sections",
                type: "time(6)",
                nullable: true,
                comment: "节次结束时间",
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_Sections",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Sections",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Schools",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PromotionSlogan",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                comment: "宣传语",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Schools",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_Schools",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_Schools",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                comment: "简介",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_Schools",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Badge",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                comment: "校徽",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AppX_Schools",
                type: "longtext",
                nullable: false,
                comment: "校址",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Schools",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_SchoolCampuses",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_SchoolCampuses",
                type: "bigint",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_SchoolCampuses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_SchoolCampuses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_SchoolCampuses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_SchoolCampuses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "AppX_SchoolCampuses",
                type: "longtext",
                nullable: false,
                comment: "校址",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_SchoolCampuses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_SchoolBuildings",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_SchoolBuildings",
                type: "bigint",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_SchoolBuildings",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_SchoolBuildings",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_SchoolBuildings",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Schedules",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_Schedules",
                type: "int",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Schedules",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "节次方案名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LayoutOfWeek",
                table: "AppX_Schedules",
                type: "longtext",
                nullable: false,
                comment: "布局节次表",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_Schedules",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_Schedules",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_Schedules",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Schedules",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编号",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_RoomUsages",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_RoomUsages",
                type: "int",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_RoomUsages",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_RoomUsages",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_RoomUsages",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_RoomUsages",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_RoomUsages",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Projects",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Projects",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "项目名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_Projects",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Courses",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "AppX_Courses",
                type: "int",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Courses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "课程名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_Courses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_Courses",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_Courses",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Courses",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编号",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_Classrooms",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UsageCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                comment: "用途编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                comment: "学校编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                comment: "校区编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_Classrooms",
                type: "bigint",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_Classrooms",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_Classrooms",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FloorCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                comment: "楼层编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_Classrooms",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingCode",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                nullable: false,
                comment: "楼栋编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_Classrooms",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "ValidState",
                table: "AppX_BuildingFloors",
                type: "int",
                nullable: false,
                comment: "数据有效标识",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCode",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                nullable: false,
                comment: "学校编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolCampusCode",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                nullable: false,
                comment: "校区编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Order",
                table: "AppX_BuildingFloors",
                type: "bigint",
                nullable: false,
                comment: "序号",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AppX_BuildingFloors",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AppX_BuildingFloors",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AppX_BuildingFloors",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingCode",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                nullable: false,
                comment: "楼栋编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppX_BuildingFloors",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                comment: "编码",
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUserTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpUserTokens",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AbpUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUserRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AbpUserRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpUserRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUserLogins",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpUserLogins",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TargetUserId",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SourceUserId",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpUserDelegations",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpUserClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpUserClaims",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpUserClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpTenants",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpTenantConnectionStrings",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpSettings",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpSettingDefinitions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpSessions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpSessions",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpSessions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpSecurityLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpSecurityLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpSecurityLogs",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpRoleClaims",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AbpRoleClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpRoleClaims",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpPermissions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpPermissionGroups",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpPermissionGrants",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpPermissionGrants",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ParentId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifierId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DeleterId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpOrganizationUnits",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TargetUserId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TargetTenantId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SourceUserId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "SourceTenantId",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpLinkUsers",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpFeatureValues",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpFeatures",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpFeatureGroups",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpEntityPropertyChanges",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EntityChangeId",
                table: "AbpEntityPropertyChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpEntityPropertyChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EntityTenantId",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AuditLogId",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpEntityChanges",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpClaimTypes",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpBackgroundJobs",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ImpersonatorUserId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ImpersonatorTenantId",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpAuditLogs",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TenantId",
                table: "AbpAuditLogActions",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AuditLogId",
                table: "AbpAuditLogActions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AbpAuditLogActions",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
