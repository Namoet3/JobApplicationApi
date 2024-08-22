using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPageUrlToActionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageUrl",
                table: "ActionLogs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageUrl",
                table: "ActionLogs");
        }
    }
}
