using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartsManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageLocationToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StorageLocation",
                table: "Inv_Transactions",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageLocation",
                table: "Inv_CurrentStock",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 1L,
                column: "StorageLocation",
                value: "A-01");

            migrationBuilder.UpdateData(
                table: "Inv_CurrentStock",
                keyColumn: "StockID",
                keyValue: 2L,
                column: "StorageLocation",
                value: "A-02");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageLocation",
                table: "Inv_Transactions");

            migrationBuilder.DropColumn(
                name: "StorageLocation",
                table: "Inv_CurrentStock");
        }
    }
}
