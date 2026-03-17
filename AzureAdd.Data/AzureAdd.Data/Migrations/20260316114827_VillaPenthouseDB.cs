using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureAdd.Data.Migrations
{
    /// <inheritdoc />
    public partial class VillaPenthouseDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerNight",
                table: "VillasPenthhouses",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPricePrice",
                table: "Bookings",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92e3292d-4e52-48af-b640-657600c3261c", "AQAAAAIAAYagAAAAEPigo/JRhiLARrgcJrHnO3ykLNEpYWXUIOVLBKNKu1lcKo3Fw1yv3hqmdiDcRed9Zw==", "ecd00d4c-0efc-4d14-aeb3-d9a70947436d" });

            migrationBuilder.UpdateData(
                table: "VillasPenthhouses",
                keyColumn: "IdVilla",
                keyValue: 1,
                column: "PricePerNight",
                value: 100m);

            migrationBuilder.UpdateData(
                table: "VillasPenthhouses",
                keyColumn: "IdVilla",
                keyValue: 2,
                column: "PricePerNight",
                value: 180m);

            migrationBuilder.UpdateData(
                table: "VillasPenthhouses",
                keyColumn: "IdVilla",
                keyValue: 3,
                column: "PricePerNight",
                value: 340m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerNight",
                table: "VillasPenthhouses");

            migrationBuilder.DropColumn(
                name: "TotalPricePrice",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7699db7d-964f-4782-8209-d76562e0fece",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98776ada-0b2c-40ac-8d2b-c18b911c389a", "AQAAAAIAAYagAAAAEKNZ0JwEsm4tibh9x2ToUx4x6AOR1HYox94YM6yHW1DosUKtS7mDWxMMaOKyj+UiSg==", "ce4f3b9d-6618-46be-81b7-0005db1418f0" });
        }
    }
}
