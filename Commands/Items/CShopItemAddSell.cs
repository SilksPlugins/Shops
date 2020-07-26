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
    [CommandDescription("Adds a sellable item to the shop")]
    [CommandParent(typeof(CShopItemAdd))]
    public class CShopItemAddSell : CShopAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;

        public CShopItemAddSell(ShopsPlugin shopsPlugin,
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

            if (shop != null)
            {
                throw new UserFriendlyException("Shop already exists");
            }

            shop = new SellItem()
            {
                ID = asset.id,
                SellPrice = price
            };

            await m_DbContext.SellItemShops.AddAsync(shop);

            await m_DbContext.SaveChangesAsync();

            await Context.Actor.PrintMessageAsync("Added shop");
        }
    }
}
