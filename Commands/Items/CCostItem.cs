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
    [CommandAlias("icost")]
    [CommandAlias("costi")]
    [CommandAlias("itemcost")]
    [CommandAlias("costitem")]
    [CommandDescription("Views the cost of the specified item")]
    [CommandSyntax("<id or name>")]
    public class CCostItem : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;

        public CCostItem(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
        }

        protected override async UniTask OnExecuteAsync()
        {
            string idOrName = await Context.Parameters.GetAsync<string>(0);

            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Item not found");
            }

            BuyItem buyItem = await m_DbContext.BuyItemShops.FindAsync((int)asset.id);
            SellItem sellItem = await m_DbContext.SellItemShops.FindAsync((int)asset.id);

            if (buyItem == null && sellItem == null)
            {
                await Context.Actor.PrintMessageAsync("No shops exist");
            }
            else if (buyItem != null && sellItem == null)
            {
                await Context.Actor.PrintMessageAsync($"Buy item {asset.itemName} for {buyItem.BuyPrice}");
            }
            else if (buyItem == null && sellItem != null)
            {
                await Context.Actor.PrintMessageAsync($"Sell item {asset.itemName} for {sellItem.SellPrice}");
            }
            else
            {
                await Context.Actor.PrintMessageAsync($"Buy item {asset.itemName} for {buyItem.BuyPrice}");
                await Context.Actor.PrintMessageAsync($"Sell item {asset.itemName} for {sellItem.SellPrice}");
            }
        }
    }
}
