using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class BuildingTheBases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Events",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsersEvents",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersEvents", x => new { x.UserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_UsersEvents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c991b2cf-52ab-4bfa-8397-7f9339b23e1e", "AQAAAAIAAYagAAAAEE5GtKNqHHCAQovic7jVju2C3RirZjGN5TVVcYognsHcxPkQd4ntVngOtcpp0HYY4w==", "5e092e9b-3a87-4a1d-b1db-1d1430dc0d5b" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthorId", "ImageUrl" },
                values: new object[] { "cb79d2b9-3231-408b-83d3-30c6879aa313", null });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AuthorId", "ImageUrl" },
                values: new object[] { "929dbb49-522c-4f84-a486-5d11e35fd7fc", null });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AuthorId", "ImageUrl" },
                values: new object[] { "1d48e3d2-08e1-4162-8c51-65f0723a017f", null });

            migrationBuilder.CreateIndex(
                name: "IX_Events_AuthorId",
                table: "Events",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEvents_EventId",
                table: "UsersEvents",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_AuthorId",
                table: "Events",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_AuthorId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "UsersEvents");

            migrationBuilder.DropIndex(
                name: "IX_Events_AuthorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Events",
                newName: "Date");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "899ee380-9f38-46a9-afff-eb5c296b9ab7", "AQAAAAIAAYagAAAAEH+Ef2rumjI34HsuKKVace542bzsBAvjqDUPLYrr7Fhvzyqkti6ZR2OBcczjU0INAQ==", "fbd3dc8c-1ab8-4188-9a6f-370146f13274" });
        }
    }
}
