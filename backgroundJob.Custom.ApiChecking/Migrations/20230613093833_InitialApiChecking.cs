using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backgroundJob.Custom.ApiChecking.Migrations
{
    /// <inheritdoc />
    public partial class InitialApiChecking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AC");

            migrationBuilder.CreateTable(
                name: "Data",
                schema: "AC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UrlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pass = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Url",
                schema: "AC",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Protocol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Headers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interval = table.Column<TimeSpan>(type: "time", nullable: true),
                    ExpectedResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Url", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data",
                schema: "AC");

            migrationBuilder.DropTable(
                name: "Url",
                schema: "AC");
        }
    }
}
