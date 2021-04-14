using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("add", Priority = Priority.Normal)]
    [CommandAlias("a")]
    [CommandAlias("+")]
    [CommandSyntax("<vehicle> <price>")]
    [CommandDescription("Adds the vehicle to the shop to be bought.")]
    [CommandParent(typeof(CVShop))]
    public class CVShopAdd : ShopCommand
    {
        public CVShopAdd(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var asset = await GetVehicleAsset(0);
            var price = await GetPrice(1);

            await ShopManager.AddVehicleShopBuyable(asset.VehicleAssetId, price);

            await PrintAsync(
                StringLocalizer["commands:success:shop_added:buyable_vehicle",
                    new
                    {
                        VehicleAsset = asset,
                        Price = price,
                        EconomyProvider.CurrencySymbol,
                        EconomyProvider.CurrencyName
                    }]);
        }
    }
}