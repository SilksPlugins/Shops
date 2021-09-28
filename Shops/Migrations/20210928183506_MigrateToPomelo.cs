using Microsoft.EntityFrameworkCore.Migrations;

namespace Shops.Migrations
{
    public partial class MigrateToPomelo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Shops_VehicleShops",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "Shops_VehicleShops",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(767)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellPrice",
                table: "Shops_ItemShops",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Shops_ItemShops",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "Shops_ItemShops",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(767)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Shops_VehicleShops",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "Shops_VehicleShops",
                type: "varchar(767)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<decimal>(
                name: "SellPrice",
                table: "Shops_ItemShops",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Shops_ItemShops",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "Shops_ItemShops",
                type: "varchar(767)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
