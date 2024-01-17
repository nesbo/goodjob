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
                name: "JobStash",
                schema: "Work",
                columns: table => new
                {
                    JobStashId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStash", x => x.JobStashId);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Talent",
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
                name: "Job",
                schema: "Work",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JobStashId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    PublishedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Budget = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Skills = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Job_JobStash_JobStashId",
                        column: x => x.JobStashId,
                        principalSchema: "Work",
                        principalTable: "JobStash",
                        principalColumn: "JobStashId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonUpworkRssFeed",
                schema: "Talent",
                columns: table => new
                {
                    PersonUpworkRssFeedId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                name: "IX_Job_JobStashId",
                schema: "Work",
                table: "Job",
                column: "JobStashId");

            migrationBuilder.CreateIndex(
                name: "IX_JobStash_PersonId",
                schema: "Work",
                table: "JobStash",
                column: "PersonId",
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
                name: "JobStash",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Talent");
        }
    }
}
