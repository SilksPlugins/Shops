using Microsoft.EntityFrameworkCore.Migrations;

namespace Shops.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shops_ItemShops",
                columns: table => new
                {
                    ItemId = table.Column<string>(maxLength: 128, nullable: false),
                    BuyPrice = table.Column<decimal>(nullable: true),
                    SellPrice = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops_ItemShops", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Shops_VehicleShops",
                columns: table => new
                {
                    VehicleId = table.Column<string>(maxLength: 128, nullable: false),
                    BuyPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops_VehicleShops", x => x.VehicleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shops_ItemShops");

            migrationBuilder.DropTable(
                name: "Shops_VehicleShops");
        }
    }
}
