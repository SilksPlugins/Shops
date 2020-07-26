using Cysharp.Threading.Tasks;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using Shops.Database;
using Shops.Database.Models;
using System;

namespace Shops.Commands.Items
{
    [Command("cost")]
    [CommandAlias("vcost")]
    [CommandAlias("costv")]
    [CommandAlias("vehiclecost")]
    [CommandAlias("costvehicle")]
    [CommandDescription("Views the cost of the specified vehicle")]
    [CommandSyntax("<id or name>")]
    public class CCostVehicle : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;

        public CCostVehicle(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
        }

        protected override async UniTask OnExecuteAsync()
        {
            string idOrName = await Context.Parameters.GetAsync<string>(0);

            VehicleAsset asset = (VehicleAsset)m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Vehicle not found");
            }

            BuyVehicle buyVehicle = await m_DbContext.BuyVehicleShops.FindAsync((int)asset.id);

            if (buyVehicle == null)
            {
                await Context.Actor.PrintMessageAsync("No shops exist");
            }
            else
            {
                await Context.Actor.PrintMessageAsync($"Buy vehicle {asset.vehicleName} for {buyVehicle.BuyPrice}");
            }
        }
    }
}
