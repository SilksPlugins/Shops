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
    [Command("buy")]
    [CommandAlias("b")]
    [CommandSyntax("<id or name>")]
    [CommandParent(typeof(CShopItemRemove))]
    public class CShopItemRemoveBuy : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;

        public CShopItemRemoveBuy(ShopsPlugin shopsPlugin,
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
                throw new UserFriendlyException("Item not found");
            }

            BuyItem shop = await m_DbContext.BuyItemShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException("Shop doesn't exist");
            }

            m_DbContext.BuyItemShops.Remove(shop);

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync("Removed shop");
        }
    }
}
