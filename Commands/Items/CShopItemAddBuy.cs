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

namespace Shops.Commands.Items
{
    [Command("buy")]
    [CommandAlias("b")]
    [CommandSyntax("<id or name> <price>")]
    [CommandDescription("Adds a buyable item to the shop")]
    [CommandParent(typeof(CShopItemAdd))]
    public class CShopItemAddBuy : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CShopItemAddBuy(ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            ShopDbContext dbContext,
            IEconomyProvider economyProvider,
            IServiceProvider serviceProvider) : base(stringLocalizer, serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
            m_EconomyProvider = economyProvider;
        }

        protected override async Task ExecuteShopUpdateAsync(string idOrName, decimal price)
        {
            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IDOrName = idOrName }]);
            }

            BuyItem shop = await m_DbContext.BuyItemShops.FindAsync((int)asset.id);

            if (shop != null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_buy_shop_already_exists",
                    new
                    {
                        ItemName = asset.itemName,
                        ItemID = asset.id,
                        shop.BuyPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }

            shop = new BuyItem()
            {
                ID = asset.id,
                BuyPrice = price
            };

            await m_DbContext.BuyItemShops.AddAsync(shop);

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_buy_shop_added",
                new
                {
                    ItemName = asset.itemName,
                    ItemID = asset.id,
                    shop.BuyPrice,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol
                }]);
        }
    }
}
