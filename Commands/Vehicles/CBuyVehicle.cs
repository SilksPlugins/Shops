using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using SDG.Unturned;
using Shops.Commands.Actions;
using Shops.Shops;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("buyv")]
    [CommandAlias("vbuy")]
    [CommandAlias("buyvehicle")]
    [CommandAlias("vehiclebuy")]
    [CommandDescription("Buys the specified vehicle")]
    [CommandSyntax("<id or name>")]
    public class CBuyVehicle : CInteractAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;

        public CBuyVehicle(ShopsPlugin shopsPlugin,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
        }

        protected override async Task ExecuteInteractAsync(string identifier, int amount)
        {
            if (amount != 1)
            {
                throw new UserFriendlyException("You may only purchase one vehicle at a time.");
            }

            Asset asset = m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, identifier);

            if (asset == null)
            {
                throw new UserFriendlyException("Vehicle not found");
            }

            ShopBuyVehicle shop = await m_ShopsPlugin.GetBuyVehicleShop(asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException("Vehicle not for sale");
            }

            await shop.Interact(User);
        }
    }
}
