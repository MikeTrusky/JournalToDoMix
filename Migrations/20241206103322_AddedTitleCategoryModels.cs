using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JournalToDoMix.Migrations
{
    /// <inheritdoc />
    public partial class AddedTitleCategoryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Activities");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ActivityCategoryId",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActivityTitleId",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActivityCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTitles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ActivityCategories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Reading" },
                    { 2, "Learning" },
                    { 3, "Gaming" },
                    { 4, "Chilling" },
                    { 5, "Physical working" }
                });

            migrationBuilder.InsertData(
                table: "ActivityTitles",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Spending some time on reading a book", "Reading a book" },
                    { 2, "Testing some new drawing methods", "Learning to draw" },
                    { 3, "Playing a new pc game", "Playing a game" },
                    { 4, "Programming new big project", "Programming an app" },
                    { 5, "Watching an outside rain", "Chilling in a rain" },
                    { 6, "Walking around the room", "Chilling in a room" },
                    { 7, "Painting a garage wall", "Painting the wall" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "ActivityCategoryId", "ActivityTitleId", "Description", "DurationPlanned", "IsCompleted", "StartedAt" },
                values: new object[,]
                {
                    { 11, 1, 1, "", new TimeSpan(0, 0, 30, 0, 0), true, new DateTime(2024, 10, 18, 15, 0, 0, 0, DateTimeKind.Local) },
                    { 12, 3, 3, "", new TimeSpan(0, 0, 30, 0, 0), true, new DateTime(2024, 10, 23, 10, 50, 0, 0, DateTimeKind.Local) },
                    { 13, 2, 4, "", new TimeSpan(0, 1, 0, 0, 0), true, new DateTime(2024, 10, 23, 10, 0, 0, 0, DateTimeKind.Local) },
                    { 14, 4, 6, "", new TimeSpan(0, 0, 30, 0, 0), true, new DateTime(2024, 12, 3, 13, 30, 0, 0, DateTimeKind.Local) },
                    { 15, 4, 5, "", new TimeSpan(0, 1, 0, 0, 0), true, new DateTime(2024, 12, 3, 13, 0, 0, 0, DateTimeKind.Local) },
                    { 16, 4, 5, "", new TimeSpan(0, 1, 0, 0, 0), true, new DateTime(2024, 12, 3, 13, 0, 0, 0, DateTimeKind.Local) },
                    { 17, 5, 7, "", new TimeSpan(0, 5, 0, 0, 0), true, new DateTime(2024, 11, 5, 10, 0, 0, 0, DateTimeKind.Local) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityCategoryId",
                table: "Activities",
                column: "ActivityCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTitleId",
                table: "Activities",
                column: "ActivityTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCategories_CategoryName",
                table: "ActivityCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTitles_Title",
                table: "ActivityTitles",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityCategories_ActivityCategoryId",
                table: "Activities",
                column: "ActivityCategoryId",
                principalTable: "ActivityCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityTitles_ActivityTitleId",
                table: "Activities",
                column: "ActivityTitleId",
                principalTable: "ActivityTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityCategories_ActivityCategoryId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityTitles_ActivityTitleId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "ActivityCategories");

            migrationBuilder.DropTable(
                name: "ActivityTitles");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ActivityCategoryId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ActivityTitleId",
                table: "Activities");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DropColumn(
                name: "ActivityCategoryId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ActivityTitleId",
                table: "Activities");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Activities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Activities",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Description", "DurationPlanned", "IsCompleted", "StartedAt", "Title" },
                values: new object[] { 1, "Reading a book", new TimeSpan(0, 0, 30, 0, 0), false, new DateTime(2024, 10, 18, 15, 0, 0, 0, DateTimeKind.Local), "Reading" });
        }
    }
}
