using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profkom.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VolunteerQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    VolunteerId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QuestionTitle = table.Column<string>(type: "text", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerQuestions_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerRequestLeaving",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    VolunteerId = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerRequestLeaving", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerRequestLeaving_VolunteerRequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "VolunteerRequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerRequestLeaving_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteerAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    QuestionId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Answer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerAnswers_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerAnswers_VolunteerQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "VolunteerQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAnswers_QuestionId",
                table: "VolunteerAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerAnswers_UserId",
                table: "VolunteerAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerQuestions_VolunteerId",
                table: "VolunteerQuestions",
                column: "VolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerRequestLeaving_StatusId",
                table: "VolunteerRequestLeaving",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerRequestLeaving_VolunteerId",
                table: "VolunteerRequestLeaving",
                column: "VolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteerAnswers");

            migrationBuilder.DropTable(
                name: "VolunteerRequestLeaving");

            migrationBuilder.DropTable(
                name: "VolunteerQuestions");
        }
    }
}
