using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backgroundJob.Custom.ApiChecking.Migrations
{
    /// <inheritdoc />
    public partial class AddWaitResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WaitResult",
                schema: "AC",
                table: "Url",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaitResult",
                schema: "AC",
                table: "Url");
        }
    }
}
