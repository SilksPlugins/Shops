using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using Shops.Database;
using Shops.Database.Models;
using System;

namespace Shops.Commands.Vehicles
{
    [Command("costv")]
    [CommandAlias("vcost")]
    [CommandAlias("costvehicle")]
    [CommandAlias("vehiclecost")]
    [CommandDescription("Views the cost of the specified vehicle")]
    [CommandSyntax("<id or name>")]
    public class CCostVehicle : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CCostVehicle(ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            ShopDbContext dbContext,
            IEconomyProvider economyProvider,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_StringLocalizer = stringLocalizer;
            m_DbContext = dbContext;
            m_EconomyProvider = economyProvider;
        }

        protected override async UniTask OnExecuteAsync()
        {
            string idOrName = await Context.Parameters.GetAsync<string>(0);

            VehicleAsset asset = (VehicleAsset)m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["vehicle_not_found", new { IDOrName = idOrName }]);
            }

            BuyVehicle buyVehicle = null;

            if (m_ShopsPlugin.CanBuyVehicles)
            {
                buyVehicle = await m_DbContext.BuyVehicleShops.FindAsync((int)asset.id);
            }

            if (buyVehicle == null)
            {
                await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:vehicle_cost_none", new { VehicleName = asset.vehicleName, VehicleID = asset.id }]);
            }
            else
            {
                await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:vehicle_cost_buy",
                    new
                    {
                        VehicleName = asset.vehicleName,
                        VehicleID = asset.id,
                        buyVehicle.BuyPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }
        }
    }
}
