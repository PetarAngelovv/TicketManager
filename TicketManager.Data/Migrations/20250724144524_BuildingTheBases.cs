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
                    { "b3102d7f-82cb-470c-88d3-a299a1e8c1b9", 0, "5916546b-da9c-4f53-b952-022fe6c8fe81", "user@TManager.com", true, false, null, "USER@TMANAGER.COM", "USER@TMANAGER.COM", "AQAAAAIAAYagAAAAELmDGs6dnuItSs7dClrdxm1+MG95vAxg7q6Ezcj2+Z8EWEjyCC2nxOYcKegEbm0AQw==", null, false, "f24e6608-b03d-46ba-9cc0-b714ca5aed92", false, "user@TManager.com" },
                    { "e14720aa-73e1-4f8f-8f1f-6736cfe1a00b", 0, "ec1a5ca4-c32c-45ad-90df-e29862bcf2e9", "manager@TManager.com", true, false, null, "MANAGER@TMANAGER.COM", "MANAGER@TMANAGER.COM", "AQAAAAIAAYagAAAAEGbEELRoSGlT2i4pNT6pCPUb3CkfVqnP0m0ulioM9KGSzPLUB/cyPZP+CyzKc8R4hQ==", null, false, "8191db06-d8be-420e-86e8-b3c4fcc329c1", false, "manager@TManager.com" },
                    { "fcf6a048-50ce-4fd6-a89b-2d95c88e607a", 0, "c8a7d16c-9d16-4f7d-9bc8-2c126af365e6", "admin@TManager.com", true, false, null, "ADMIN@TMANAGER.COM", "ADMIN@TMANAGER.COM", "AQAAAAIAAYagAAAAEME14QEIbUqQlUZBPBeDT89KGvCoHIfP14c04AeHnPN4Bz1V7JmGzUrEOH5EbRhU2w==", null, false, "452a25a3-c2a0-4ea5-82e3-7ac91fb0f5bf", false, "admin@TManager.com" }
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
