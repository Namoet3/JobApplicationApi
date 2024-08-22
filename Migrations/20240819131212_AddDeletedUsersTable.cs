using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "SkillCertificates",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "References",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Memberships",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Languages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "JobExperiences",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeletedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserDetailsId = table.Column<int>(type: "integer", nullable: false),
                    EducationLevel = table.Column<string>(type: "text", nullable: true),
                    Highschool = table.Column<string>(type: "text", nullable: true),
                    University = table.Column<string>(type: "text", nullable: true),
                    Program = table.Column<string>(type: "text", nullable: true),
                    GraduateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gpa = table.Column<string>(type: "text", nullable: true),
                    MilitaryStatus = table.Column<string>(type: "text", nullable: true),
                    MilitaryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HealthStat = table.Column<string>(type: "text", nullable: true),
                    Health = table.Column<string>(type: "text", nullable: true),
                    Disability = table.Column<string>(type: "text", nullable: true),
                    Criminal = table.Column<string>(type: "text", nullable: true),
                    CriminalDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CriminalReason = table.Column<string>(type: "text", nullable: true),
                    CriminalRecordFile = table.Column<string>(type: "text", nullable: true),
                    Skills = table.Column<string>(type: "text", nullable: true),
                    Hobbies = table.Column<string>(type: "text", nullable: true),
                    CvFile = table.Column<string>(type: "text", nullable: true),
                    Policy = table.Column<bool>(type: "boolean", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeletedUsers_UserDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillCertificates_DeletedUserId",
                table: "SkillCertificates",
                column: "DeletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_References_DeletedUserId",
                table: "References",
                column: "DeletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_DeletedUserId",
                table: "Memberships",
                column: "DeletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_DeletedUserId",
                table: "Languages",
                column: "DeletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobExperiences_DeletedUserId",
                table: "JobExperiences",
                column: "DeletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeletedUsers_UserDetailsId",
                table: "DeletedUsers",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobExperiences_DeletedUsers_DeletedUserId",
                table: "JobExperiences",
                column: "DeletedUserId",
                principalTable: "DeletedUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_DeletedUsers_DeletedUserId",
                table: "Languages",
                column: "DeletedUserId",
                principalTable: "DeletedUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_DeletedUsers_DeletedUserId",
                table: "Memberships",
                column: "DeletedUserId",
                principalTable: "DeletedUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_References_DeletedUsers_DeletedUserId",
                table: "References",
                column: "DeletedUserId",
                principalTable: "DeletedUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillCertificates_DeletedUsers_DeletedUserId",
                table: "SkillCertificates",
                column: "DeletedUserId",
                principalTable: "DeletedUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobExperiences_DeletedUsers_DeletedUserId",
                table: "JobExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_Languages_DeletedUsers_DeletedUserId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_DeletedUsers_DeletedUserId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_References_DeletedUsers_DeletedUserId",
                table: "References");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillCertificates_DeletedUsers_DeletedUserId",
                table: "SkillCertificates");

            migrationBuilder.DropTable(
                name: "DeletedUsers");

            migrationBuilder.DropIndex(
                name: "IX_SkillCertificates_DeletedUserId",
                table: "SkillCertificates");

            migrationBuilder.DropIndex(
                name: "IX_References_DeletedUserId",
                table: "References");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_DeletedUserId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Languages_DeletedUserId",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_JobExperiences_DeletedUserId",
                table: "JobExperiences");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "SkillCertificates");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "References");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "JobExperiences");
        }
    }
}
