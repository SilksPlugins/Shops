using OpenMod.API.Commands;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("vcost", Priority = Priority.Normal)]
    [CommandAlias("vehiclecost")]
    [CommandAlias("costv")]
    [CommandAlias("costvehicle")]
    [CommandSyntax("<vehicle>")]
    [CommandDescription("Checks the price of a vehicle in the shop.")]
    public class CVCost : ShopCommand
    {
        public CVCost(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            AssertCanBuyVehicles();

            var asset = await GetVehicleAsset(0);

            var shop = await ShopManager.GetVehicleShopData(asset.VehicleAssetId);

            if (shop == null)
                throw new UserFriendlyException(
                    StringLocalizer["commands:errors:no_vehicle_shop", new { ItemAsset = asset }]);

            await PrintAsync(StringLocalizer["commands:success:vehicle_cost:buy",
                new
                {
                    VehicleAsset = asset,
                    shop.BuyPrice,
                    EconomyProvider.CurrencySymbol,
                    EconomyProvider.CurrencyName
                }]);
        }
    }
}
