using Microsoft.EntityFrameworkCore.Migrations;

namespace ScheduleTask.Migrations
{
    public partial class changeNewTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Hour",
                table: "Tasks",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Minute",
                table: "Tasks",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07351063-71c9-48bc-9a84-60c1e2711ca1",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "d3d64608-fc7c-4324-b114-24f905a9fbc3", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a597799a-fb1b-44b2-939f-48731dec318e",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "34b85ea5-7dce-4248-be50-9f7f7fed1613", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "07351063-71c9-48bc-9a84-60c1e27111ca",
                column: "ConcurrencyStamp",
                value: "2bd515e7-c0a8-46f0-8692-d0e2ace31255");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hour",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Minute",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07351063-71c9-48bc-9a84-60c1e2711ca1",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "50c5804e-2839-4f3d-9cf0-cabf0c80488d", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a597799a-fb1b-44b2-939f-48731dec318e",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "8f054f6a-e082-4db7-b2e6-8a5e3d80b43f", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "07351063-71c9-48bc-9a84-60c1e27111ca",
                column: "ConcurrencyStamp",
                value: "12b2486e-802d-48d2-9349-d4c48cbfa44e");
        }
    }
}
