using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kontravers.GoodJob.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrefferedProfileIdRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferredPortfolioId",
                schema: "Talent",
                table: "PersonUpworkRssFeed",
                newName: "PreferredProfileId");

            migrationBuilder.RenameColumn(
                name: "PreferredPortfolioId",
                schema: "Work",
                table: "Job",
                newName: "PreferredProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferredProfileId",
                schema: "Talent",
                table: "PersonUpworkRssFeed",
                newName: "PreferredPortfolioId");

            migrationBuilder.RenameColumn(
                name: "PreferredProfileId",
                schema: "Work",
                table: "Job",
                newName: "PreferredPortfolioId");
        }
    }
}
