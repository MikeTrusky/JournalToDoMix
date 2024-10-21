using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JournalToDoMix.Migrations
{
    /// <inheritdoc />
    public partial class RenameActionToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationPlanned = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Description", "DurationPlanned", "IsCompleted", "StartedAt", "Title" },
                values: new object[] { 1, "Reading a book", new TimeSpan(0, 0, 30, 0, 0), false, new DateTime(2024, 10, 18, 15, 0, 0, 0, DateTimeKind.Local), "Reading" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationPlanned = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "Id", "Description", "DurationPlanned", "IsCompleted", "StartedAt", "Title" },
                values: new object[] { 1, "Reading a book", new TimeSpan(0, 0, 30, 0, 0), false, new DateTime(2024, 10, 18, 15, 0, 0, 0, DateTimeKind.Local), "Reading" });
        }
    }
}
