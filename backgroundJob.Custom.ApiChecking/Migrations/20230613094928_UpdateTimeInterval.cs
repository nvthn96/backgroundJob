using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backgroundJob.Custom.ApiChecking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeInterval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Interval",
                schema: "AC",
                table: "Url");

            migrationBuilder.AddColumn<int>(
                name: "IntervalInMinutes",
                schema: "AC",
                table: "Url",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntervalInMinutes",
                schema: "AC",
                table: "Url");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Interval",
                schema: "AC",
                table: "Url",
                type: "time",
                nullable: true);
        }
    }
}
