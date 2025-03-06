using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMvc.Migrations
{
    /// <inheritdoc />
    public partial class OtherEntitie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_Seller_sellerId",
                table: "SalesRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesRecords",
                table: "SalesRecords");

            migrationBuilder.RenameTable(
                name: "SalesRecords",
                newName: "SalesRecord");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "SalesRecord",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "sellerId",
                table: "SalesRecord",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesRecords_sellerId",
                table: "SalesRecord",
                newName: "IX_SalesRecord_SellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesRecord",
                table: "SalesRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecord_Seller_SellerId",
                table: "SalesRecord",
                column: "SellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecord_Seller_SellerId",
                table: "SalesRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesRecord",
                table: "SalesRecord");

            migrationBuilder.RenameTable(
                name: "SalesRecord",
                newName: "SalesRecords");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "SalesRecords",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "SalesRecords",
                newName: "sellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesRecord_SellerId",
                table: "SalesRecords",
                newName: "IX_SalesRecords_sellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesRecords",
                table: "SalesRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_Seller_sellerId",
                table: "SalesRecords",
                column: "sellerId",
                principalTable: "Seller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
