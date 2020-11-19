using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Economy.Abstractions;
using SDG.Unturned;
using Shops.Commands.Actions;
using Shops.Database;
using Shops.Database.Models;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("add")]
    [CommandAlias("a")]
    [CommandAlias("+")]
    [CommandSyntax("<id or name> <cost>")]
    [CommandParent(typeof(CShopVehicle))]
    public class CShopVehicleAdd : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CShopVehicleAdd(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IEconomyProvider economyProvider,
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(stringLocalizer, serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
            m_EconomyProvider = economyProvider;
        }

        protected override async Task ExecuteShopUpdateAsync(string idOrName, decimal price)
        {
            VehicleAsset asset = (VehicleAsset)m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["vehicle_not_found", new { IdOrName = idOrName }]);
            }

            BuyVehicle shop = await m_DbContext.BuyVehicleShops.FindAsync((int)asset.id);

            if (shop != null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:vehicle_buy_shop_already_exists",
                    new
                    {
                        VehicleName = asset.vehicleName,
                        ItemId = asset.id,
                        shop.BuyPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }

            shop = new BuyVehicle()
            {
                Id = asset.id,
                BuyPrice = price
            };

            await m_DbContext.BuyVehicleShops.AddAsync(shop);

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:vehicle_buy_shop_added",
                new
                {
                    VehicleName = asset.vehicleName,
                    VehicleId = asset.id,
                    shop.BuyPrice,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol
                }]);
        }
    }
}
