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
    [Command("sell")]
    [CommandAlias("s")]
    [CommandSyntax("<id or name> <price>")]
    [CommandDescription("Adds a sellable item to the shop")]
    [CommandParent(typeof(CShopItemAdd))]
    public class CShopItemAddSell : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CShopItemAddSell(ShopsPlugin shopsPlugin,
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
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IdOrName = idOrName }]);
            }

            SellItem shop = await m_DbContext.SellItemShops.FindAsync((int)asset.id);

            if (shop != null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_sell_shop_already_exists",
                    new
                    {
                        ItemName = asset.itemName,
                        ItemId = asset.id,
                        shop.SellPrice,
                        m_EconomyProvider.CurrencyName,
                        m_EconomyProvider.CurrencySymbol
                    }]);
            }

            shop = new SellItem()
            {
                Id = asset.id,
                SellPrice = price
            };

            await m_DbContext.SellItemShops.AddAsync(shop);

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_sell_shop_added",
                new
                {
                    ItemName = asset.itemName,
                    ItemId = asset.id,
                    shop.SellPrice,
                    m_EconomyProvider.CurrencyName,
                    m_EconomyProvider.CurrencySymbol
                }]);
        }
    }
}
