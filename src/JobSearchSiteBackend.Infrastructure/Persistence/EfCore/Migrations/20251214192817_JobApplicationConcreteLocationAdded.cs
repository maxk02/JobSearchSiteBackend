using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearchSiteBackend.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class JobApplicationConcreteLocationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_LocationId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "UserProfiles");

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "JobApplications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_LocationId",
                table: "JobApplications",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Locations_LocationId",
                table: "JobApplications",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Locations_LocationId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_LocationId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "JobApplications");

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "UserProfiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_LocationId",
                table: "UserProfiles",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
