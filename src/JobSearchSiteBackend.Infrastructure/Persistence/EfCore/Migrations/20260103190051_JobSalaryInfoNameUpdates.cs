using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearchSiteBackend.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class JobSalaryInfoNameUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSalaryInfo_Jobs_JobId",
                table: "JobSalaryInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSalaryInfo",
                table: "JobSalaryInfo");

            migrationBuilder.RenameTable(
                name: "JobSalaryInfo",
                newName: "JobSalaryInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSalaryInfos",
                table: "JobSalaryInfos",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSalaryInfos_Jobs_JobId",
                table: "JobSalaryInfos",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSalaryInfos_Jobs_JobId",
                table: "JobSalaryInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSalaryInfos",
                table: "JobSalaryInfos");

            migrationBuilder.RenameTable(
                name: "JobSalaryInfos",
                newName: "JobSalaryInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSalaryInfo",
                table: "JobSalaryInfo",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSalaryInfo_Jobs_JobId",
                table: "JobSalaryInfo",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
