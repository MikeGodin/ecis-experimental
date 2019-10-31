using Microsoft.EntityFrameworkCore.Migrations;

namespace hedwig.Migrations
{
    public partial class AddSiteToSiteOwnedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Site_SiteId",
                table: "Enrollment");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Site",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Family",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Child",
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

            migrationBuilder.CreateIndex(
                name: "IX_Family_SiteId",
                table: "Family",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Child_SiteId",
                table: "Child",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Child_Site_SiteId",
                table: "Child",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Site_SiteId",
                table: "Enrollment",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Family_Site_SiteId",
                table: "Family",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Child_Site_SiteId",
                table: "Child");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Site_SiteId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Family_Site_SiteId",
                table: "Family");

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

            migrationBuilder.DropIndex(
                name: "IX_Family_SiteId",
                table: "Family");

            migrationBuilder.DropIndex(
                name: "IX_Child_SiteId",
                table: "Child");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Funding");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "FamilyDetermination");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Family");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Child");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Site",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Site_SiteId",
                table: "Enrollment",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
