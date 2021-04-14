using OpenMod.API.Commands;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("vbuy", Priority = Priority.Normal)]
    [CommandAlias("vehiclebuy")]
    [CommandAlias("buyv")]
    [CommandAlias("buyvehicle")]
    [CommandSyntax("<vehicle>")]
    [CommandDescription("Buys the item from the shop.")]
    public class CVBuy : ShopCommand
    {
        public CVBuy(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            AssertCanBuyVehicles();

            var asset = await GetVehicleAsset(0);

            var shop = await ShopManager.GetVehicleShop(asset.VehicleAssetId)
                       ?? throw new UserFriendlyException(
                           StringLocalizer["commands:errors:no_buyable_vehicle_shop", new { VehicleAsset = asset }]);

            var balance = await shop.Buy(GetPlayerUser());

            await PrintAsync(StringLocalizer["commands:success:vehicle_bought",
                new
                {
                    VehicleAsset = asset,
                    Price = shop.ShopData.BuyPrice,
                    Balance = balance,
                    EconomyProvider.CurrencySymbol,
                    EconomyProvider.CurrencyName
                }]);
        }
    }
}
