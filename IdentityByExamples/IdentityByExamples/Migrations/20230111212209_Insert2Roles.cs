using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityByExamples.Migrations
{
    public partial class Insert2Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5149875c-c951-4af8-a955-7fd4613407fb", "a5195215-39e9-4fbd-a314-166be7c8f5c4", "Visitor", "VISITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2ff4eebe-34fe-4ec6-8b85-07534ad8c430", "c5a44b4a-9623-4c2f-a29f-3e7c52ecc080", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ff4eebe-34fe-4ec6-8b85-07534ad8c430");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5149875c-c951-4af8-a955-7fd4613407fb");
        }
    }
}
