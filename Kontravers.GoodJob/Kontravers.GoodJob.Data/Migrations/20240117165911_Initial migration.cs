using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kontravers.GoodJob.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Work");

            migrationBuilder.EnsureSchema(
                name: "Talent");

            migrationBuilder.CreateTable(
                name: "Job",
                schema: "Work",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsertedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    PublishedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Budget = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Skills = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Uuid = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Talent",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsertedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                schema: "Talent",
                columns: table => new
                {
                    PersonUpworkRssFeedId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InsertedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false),
                    RootUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RelativeUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    LastFetchedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MinFetchIntervalInMinutes = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonUpworkRssFeed", x => x.PersonUpworkRssFeedId);
                    table.ForeignKey(
                        name: "FK_PersonUpworkRssFeed_Person_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "Talent",
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_Uuid_PersonId",
                schema: "Work",
                table: "Job",
                columns: new[] { "Uuid", "PersonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_Email_OrganisationId",
                schema: "Talent",
                table: "Person",
                columns: new[] { "Email", "OrganisationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonUpworkRssFeed_PersonId_RootUrl_RelativeUrl",
                schema: "Talent",
                table: "PersonUpworkRssFeed",
                columns: new[] { "PersonId", "RootUrl", "RelativeUrl" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "PersonUpworkRssFeed",
                schema: "Talent");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Talent");
        }
    }
}
