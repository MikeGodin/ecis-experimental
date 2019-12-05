using Microsoft.EntityFrameworkCore.Migrations;

namespace hedwig.Migrations
{
  public partial class BootstrapTestData : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.InsertData(
          table: "User",
          columns: new string[] { "FirstName", "LastName", "WingedKeysId" },
          values: new object[] { "Test", "User", "00000000-0000-0000-0000-000000000000" }
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
