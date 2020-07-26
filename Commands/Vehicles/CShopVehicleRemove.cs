using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using Shops.Database;
using Shops.Database.Models;
using System;

namespace Shops.Commands.Vehicles
{
    [Command("remove")]
    [CommandAlias("r")]
    [CommandAlias("rem")]
    [CommandAlias("-")]
    [CommandSyntax("<id or name>")]
    [CommandParent(typeof(CShopVehicle))]
    public class CShopVehicleRemove : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ShopDbContext m_DbContext;

        public CShopVehicleRemove(ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_StringLocalizer = stringLocalizer;
            m_DbContext = dbContext;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Length != 1)
            {
                throw new CommandWrongUsageException(Context);
            }

            string idOrName = await Context.Parameters.GetAsync<string>(0);

            VehicleAsset asset = (VehicleAsset)m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["vehicle_not_found", new { IDOrName = idOrName }]);
            }

            BuyVehicle shop = await m_DbContext.BuyVehicleShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:vehicle_buy_shop_doesnt_exist", new { VehicleName = asset.vehicleName, VehicleID = asset.id }]);
            }

            m_DbContext.BuyVehicleShops.Remove(shop);

            await m_DbContext.SaveChangesAsync();

            throw new UserFriendlyException(m_StringLocalizer["shops:success:vehicle_buy_shop_removed", new { VehicleName = asset.vehicleName, VehicleID = asset.id }]);
        }
    }
}
