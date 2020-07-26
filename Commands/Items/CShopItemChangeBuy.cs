using OpenMod.API.Commands;
using OpenMod.Core.Commands;
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
    [CommandDescription("Changes a buyable item in the shop")]
    [CommandParent(typeof(CShopItemChange))]
    public class CShopItemChangeBuy : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;

        public CShopItemChangeBuy(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
        }

        protected override async Task ExecuteShopUpdateAsync(string idOrName, decimal price)
        {
            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Item not found");
            }

            BuyItem shop = await m_DbContext.BuyItemShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                shop = new BuyItem()
                {
                    ID = asset.id,
                    BuyPrice = price
                };

                await m_DbContext.BuyItemShops.AddAsync(shop);
            }
            else
            {
                shop.BuyPrice = price;

                m_DbContext.BuyItemShops.Update(shop);
            }

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync("Changed shop");
        }
    }
}
