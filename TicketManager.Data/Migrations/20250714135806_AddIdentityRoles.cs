using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1b2c3d4-5678-1234-abcd-1234567890ab", null, "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "63c995db-0d83-4359-a13c-f6026311ded3", "AQAAAAIAAYagAAAAEKDJihR3RcLGksvX93YCxFrmwcswVy7TpBi4ET91xwan7M0P7xBUEZzGJzxbcoHOpA==", "78474360-359a-43dc-82fa-b903eb87c12c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-5678-1234-abcd-1234567890ab");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "df1c3a0f-1234-4cde-bb55-d5f15a6aabcd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c991b2cf-52ab-4bfa-8397-7f9339b23e1e", "AQAAAAIAAYagAAAAEE5GtKNqHHCAQovic7jVju2C3RirZjGN5TVVcYognsHcxPkQd4ntVngOtcpp0HYY4w==", "5e092e9b-3a87-4a1d-b1db-1d1430dc0d5b" });
        }
    }
}
