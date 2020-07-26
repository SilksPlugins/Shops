using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Economy.Abstractions;
using SDG.Unturned;
using Shops.Commands.Actions;
using Shops.Database;
using Shops.Database.Models;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("change")]
    [CommandAlias("chng")]
    [CommandAlias("c")]
    [CommandSyntax("<id or name> <cost>")]
    [CommandParent(typeof(CShopVehicle))]
    public class CShopVehicleChange : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CShopVehicleChange(ShopsPlugin shopsPlugin,
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
                throw new UserFriendlyException(m_StringLocalizer["vehicle_not_found", new { IDOrName = idOrName }]);
            }

            BuyVehicle shop = await m_DbContext.BuyVehicleShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                shop = new BuyVehicle()
                {
                    ID = asset.id,
                    BuyPrice = price
                };

                await m_DbContext.BuyVehicleShops.AddAsync(shop);
            }
            else
            {
                shop.BuyPrice = price;

                m_DbContext.BuyVehicleShops.Update(shop);
            }

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:vehicle_buy_shop_changed",
                new
                {
                    VehicleName = asset.vehicleName,
                    VehicleID = asset.id,
                    shop.BuyPrice,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol
                }]);
        }
    }
}
