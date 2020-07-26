using Cysharp.Threading.Tasks;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using SDG.Unturned;
using Shops.Commands.Actions;
using Shops.Database;
using Shops.Database.Models;
using System;
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

        public CShopVehicleChange(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
        }

        protected override async Task ExecuteShopUpdateAsync(string idOrName, decimal price)
        {
            VehicleAsset asset = (VehicleAsset)m_ShopsPlugin.GetAsset(EAssetType.VEHICLE, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Vehicle not found");
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

            await Context.Actor.PrintMessageAsync("Changed shop");
        }
    }
}
