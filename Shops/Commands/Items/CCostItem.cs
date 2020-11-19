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
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CCostItem(ShopsPlugin shopsPlugin,
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

            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IdOrName = idOrName }]);
            }

            BuyItem buyItem = null;
            SellItem sellItem = null;

            if (m_ShopsPlugin.CanBuyItems)
            {
                buyItem = await m_DbContext.BuyItemShops.FindAsync((int)asset.id);

            }
            if (m_ShopsPlugin.CanSellItems)
            {
                sellItem = await m_DbContext.SellItemShops.FindAsync((int)asset.id);
            }

            if (buyItem == null && sellItem == null)
            {
                await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_cost_none", new { ItemName = asset.itemName, ItemId = asset.id }]);
            }
            else if (buyItem != null && sellItem == null)
            {
                await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_cost_buy",
                    new
                    {
                        ItemName = asset.itemName,
                        ItemId = asset.id,
                        buyItem.BuyPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }
            else if (buyItem == null && sellItem != null)
            {
                await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_cost_sell",
                    new
                    {
                        ItemName = asset.itemName,
                        ItemId = asset.id,
                        sellItem.SellPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }
            else
            {
                await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_cost_buy_sell",
                    new
                    {
                        ItemName = asset.itemName,
                        ItemId = asset.id,
                        buyItem.BuyPrice,
                        sellItem.SellPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }
        }
    }
}
