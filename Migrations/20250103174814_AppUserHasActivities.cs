using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JournalToDoMix.Migrations
{
    /// <inheritdoc />
    public partial class AppUserHasActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 11,
                column: "AppUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 12,
                column: "AppUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 13,
                column: "AppUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 14,
                column: "AppUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 15,
                column: "AppUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 16,
                column: "AppUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: 17,
                column: "AppUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AppUserId",
                table: "Activities",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_AppUserId",
                table: "Activities",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_AppUserId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_AppUserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Activities");
        }
    }
}
