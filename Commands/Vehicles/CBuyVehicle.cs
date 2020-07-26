using Microsoft.Extensions.Localization;
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
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(stringLocalizer, serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
        }

        protected override async Task ExecuteInteractAsync(string idOrName, int amount)
        {
            if (amount != 1)
            {
                throw new UserFriendlyException(m_StringLocalizer["vehicle_amount_one"]);
            }

            VehicleAsset asset = (VehicleAsset)m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["vehicle_not_found", new { IDOrName = idOrName }]);
            }

            ShopBuyVehicle shop = await m_ShopsPlugin.GetBuyVehicleShop(asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["vehicle_not_for_sale", new { VehicleName = asset.vehicleName, VehicleID = asset.id }]);
            }

            await shop.Interact(User);
        }
    }
}
