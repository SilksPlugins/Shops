using Cysharp.Threading.Tasks;
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
        private readonly ShopDbContext m_DbContext;

        public CShopVehicleRemove(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Length != 1)
            {
                throw new CommandWrongUsageException(Context);
            }

            string idOrName = await Context.Parameters.GetAsync<string>(0);

            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Vehicle not found");
            }

            BuyVehicle shop = await m_DbContext.BuyVehicleShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException("Shop doesn't exist");
            }

            m_DbContext.BuyVehicleShops.Remove(shop);

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync("Removed shop");
        }
    }
}
