using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kontravers.GoodJob.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpworkRssFeedTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Talent",
                table: "PersonUpworkRssFeed",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "UpworkRSSFeed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Talent",
                table: "PersonUpworkRssFeed");
        }
    }
}
