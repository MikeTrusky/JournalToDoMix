using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JournalToDoMix.Migrations
{
    /// <inheritdoc />
    public partial class AddedInitialActionsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actions",
                columns: new[] { "Id", "Description", "DurationPlanned", "IsCompleted", "StartedAt", "Title" },
                values: new object[] { 1, "Reading a book", new TimeSpan(0, 0, 30, 0, 0), false, new DateTime(2024, 10, 18, 15, 0, 0, 0, DateTimeKind.Local), "Reading" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
