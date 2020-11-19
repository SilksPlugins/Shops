using Microsoft.EntityFrameworkCore.Migrations;

namespace Shops.Migrations
{
    public partial class RenamedIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Shops_SellItemShops",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Shops_BuyVehicleShops",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Shops_BuyItemShops",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Shops_SellItemShops",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Shops_BuyVehicleShops",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Shops_BuyItemShops",
                newName: "ID");
        }
    }
}
