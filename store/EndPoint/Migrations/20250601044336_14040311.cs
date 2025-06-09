using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class _14040311 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "isdisabled",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isdisabled",
                table: "Users");
        }
    }
}
