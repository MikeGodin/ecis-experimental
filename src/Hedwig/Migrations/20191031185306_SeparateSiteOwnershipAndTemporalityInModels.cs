using Microsoft.EntityFrameworkCore.Migrations;

namespace hedwig.Migrations
{
    public partial class SeparateSiteOwnershipAndTemporalityInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FamilyDetermination_Site_SiteId",
                table: "FamilyDetermination");

            migrationBuilder.DropForeignKey(
                name: "FK_Funding_Site_SiteId",
                table: "Funding");

            migrationBuilder.DropIndex(
                name: "IX_Funding_SiteId",
                table: "Funding");

            migrationBuilder.DropIndex(
                name: "IX_FamilyDetermination_SiteId",
                table: "FamilyDetermination");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Funding");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "FamilyDetermination");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Funding",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "FamilyDetermination",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Funding_SiteId",
                table: "Funding",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyDetermination_SiteId",
                table: "FamilyDetermination",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyDetermination_Site_SiteId",
                table: "FamilyDetermination",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Funding_Site_SiteId",
                table: "Funding",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
