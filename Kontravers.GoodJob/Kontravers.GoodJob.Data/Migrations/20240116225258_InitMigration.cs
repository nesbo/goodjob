using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kontravers.GoodJob.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    OrganisationId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "PersonUpworkRssFeed",
                columns: table => new
                {
                    PersonUpworkRssFeedId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    LastScannedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MinFetchIntervalInMinutes = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonUpworkRssFeed", x => x.PersonUpworkRssFeedId);
                    table.ForeignKey(
                        name: "FK_PersonUpworkRssFeed_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonUpworkRssFeed_PersonId",
                table: "PersonUpworkRssFeed",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonUpworkRssFeed");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
