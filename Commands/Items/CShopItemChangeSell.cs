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
    [CommandDescription("Changes a sellable item in the shop")]
    [CommandParent(typeof(CShopItemChange))]
    public class CShopItemChangeSell : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;
        private readonly IEconomyProvider m_EconomyProvider;

        public CShopItemChangeSell(ShopsPlugin shopsPlugin,
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
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { ItemName = idOrName }]);
            }

            SellItem shop = await m_DbContext.SellItemShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                shop = new SellItem()
                {
                    Id = asset.id,
                    SellPrice = price
                };

                await m_DbContext.SellItemShops.AddAsync(shop);
            }
            else
            {
                shop.SellPrice = price;

                m_DbContext.SellItemShops.Update(shop);
            }

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync(m_StringLocalizer["shops:success:item_sell_shop_changed",
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
