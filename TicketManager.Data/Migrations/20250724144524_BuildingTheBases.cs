using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class BuildingTheBases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTickets_Orders_OrderId",
                table: "OrderTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTickets_Tickets_TicketId",
                table: "OrderTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a2b3c4d-73e1-4f8f-8f1f-6736cfe1a00b", null, "Manager", "MANAGER" },
                    { "2b3c4d5e-82cb-470c-88d3-a299a1e8c1b9", null, "User", "USER" },
                    { "9d98f75a-68cb-4b56-a6b8-b4b2a7061a9f", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "b3102d7f-82cb-470c-88d3-a299a1e8c1b9", 0, "bdff56ba-0d5e-4b06-92da-9eca9a634fe0", "user@TManager.com", true, false, null, "USER@TMANAGER.COM", "USER@TMANAGER.COM", "AQAAAAIAAYagAAAAEFy0wsXm3iSoX9QwcdyEjdMghUCdGpZsfGSzn0+vjD1GsV+9pAbO2PgnYsAZ8FnMSw==", null, false, "e26b13c6-ea92-4fa0-b71c-5638b7edd9d1", false, "user@TManager.com" },
                    { "e14720aa-73e1-4f8f-8f1f-6736cfe1a00b", 0, "f02d0b93-e39c-4e3d-b11e-82e661e5d6c9", "manager@TManager.com", true, false, null, "MANAGER@TMANAGER.COM", "MANAGER@TMANAGER.COM", "AQAAAAIAAYagAAAAENPprWPWY7xfTf+MJxmjbicrp8SHuy/n8RvKCn9tmlSDcUsZqY6vtR2TQkKmNOx9wA==", null, false, "60966d7e-6f80-4a09-806f-299b70633a43", false, "manager@TManager.com" },
                    { "fcf6a048-50ce-4fd6-a89b-2d95c88e607a", 0, "15760d52-bbbb-45e8-a1f7-56c6f2ab362a", "admin@TManager.com", true, false, null, "ADMIN@TMANAGER.COM", "ADMIN@TMANAGER.COM", "AQAAAAIAAYagAAAAEMe9rsaeZ28DsWcwVdM57AECh7DZH9CHwC1dQ08i0/2oN/+jFJVTNRTx1DWzUyu6ng==", null, false, "590dca43-d64f-48db-afb5-25a1052109ff", false, "admin@TManager.com" }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuthorId", "ImageUrl" },
                values: new object[] { "fcf6a048-50ce-4fd6-a89b-2d95c88e607a", null });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AuthorId", "ImageUrl" },
                values: new object[] { "fcf6a048-50ce-4fd6-a89b-2d95c88e607a", null });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AuthorId", "ImageUrl" },
                values: new object[] { "fcf6a048-50ce-4fd6-a89b-2d95c88e607a", null });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTickets_Orders_OrderId",
                table: "OrderTickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTickets_Tickets_TicketId",
                table: "OrderTickets",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_AuthorId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTickets_Orders_OrderId",
                table: "OrderTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTickets_Tickets_TicketId",
                table: "OrderTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "UsersEvents");

            migrationBuilder.DropIndex(
                name: "IX_Events_AuthorId",
                table: "Events");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-73e1-4f8f-8f1f-6736cfe1a00b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-82cb-470c-88d3-a299a1e8c1b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d98f75a-68cb-4b56-a6b8-b4b2a7061a9f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b3102d7f-82cb-470c-88d3-a299a1e8c1b9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e14720aa-73e1-4f8f-8f1f-6736cfe1a00b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fcf6a048-50ce-4fd6-a89b-2d95c88e607a");

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

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd", 0, "899ee380-9f38-46a9-afff-eb5c296b9ab7", "admin@TManager.com", true, false, null, "ADMIN@TMANAGER.COM", "ADMIN@TMANAGER.COM", "AQAAAAIAAYagAAAAEH+Ef2rumjI34HsuKKVace542bzsBAvjqDUPLYrr7Fhvzyqkti6ZR2OBcczjU0INAQ==", null, false, "fbd3dc8c-1ab8-4188-9a6f-370146f13274", false, "admin@TManager.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTickets_Orders_OrderId",
                table: "OrderTickets",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTickets_Tickets_TicketId",
                table: "OrderTickets",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
