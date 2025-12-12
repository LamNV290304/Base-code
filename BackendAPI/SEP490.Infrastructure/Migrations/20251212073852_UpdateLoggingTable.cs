using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEP490.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLoggingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PositionError",
                table: "LogError",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionError",
                table: "LogError");
        }
    }
}
