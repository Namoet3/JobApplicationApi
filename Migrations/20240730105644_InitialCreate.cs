using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Birthplace = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    MarriageStatus = table.Column<string>(type: "text", nullable: true),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    TcNo = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    District = table.Column<string>(type: "text", nullable: true),
                    Postal = table.Column<string>(type: "text", nullable: true),
                    TelNum = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
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
                    Policy = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Company = table.Column<string>(type: "text", nullable: true),
                    Position = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Responsibilities = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobExperiences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LanguageName = table.Column<string>(type: "text", nullable: true),
                    Proficiency = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MembershipName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferenceName = table.Column<string>(type: "text", nullable: true),
                    ReferenceCompany = table.Column<string>(type: "text", nullable: true),
                    ReferencePhone = table.Column<string>(type: "text", nullable: true),
                    ReferenceEmail = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.Id);
                    table.ForeignKey(
                        name: "FK_References_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CertificateName = table.Column<string>(type: "text", nullable: true),
                    CertificateFile = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillCertificates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobExperiences_UserId",
                table: "JobExperiences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UserId",
                table: "Languages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_References_UserId",
                table: "References",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillCertificates_UserId",
                table: "SkillCertificates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserDetailsId",
                table: "Users",
                column: "UserDetailsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobExperiences");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "SkillCertificates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserDetails");
        }
    }
}
