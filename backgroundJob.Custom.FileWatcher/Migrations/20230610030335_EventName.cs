using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backgroundJob.Custom.FileWatcher.Migrations
{
    /// <inheritdoc />
    public partial class EventName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "FW",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldName",
                schema: "FW",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                schema: "FW",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "OldName",
                schema: "FW",
                table: "Event");
        }
    }
}
