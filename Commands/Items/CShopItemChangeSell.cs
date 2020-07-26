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
    [Command("sell")]
    [CommandAlias("s")]
    [CommandSyntax("<id or name> <price>")]
    [CommandDescription("Changes a sellable item in the shop")]
    [CommandParent(typeof(CShopItemChange))]
    public class CShopItemChangeSell : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;

        public CShopItemChangeSell(ShopsPlugin shopsPlugin,
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

            SellItem shop = await m_DbContext.SellItemShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                shop = new SellItem()
                {
                    ID = asset.id,
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

            await Context.Actor.PrintMessageAsync("Changed shop");
        }
    }
}
