using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using Shops.Database;
using Shops.Database.Models;
using System;

namespace Shops.Commands.Items
{
    [Command("sell")]
    [CommandAlias("s")]
    [CommandSyntax("<id or name>")]
    [CommandParent(typeof(CShopItemRemove))]
    public class CShopItemRemoveSell : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly ShopDbContext m_DbContext;
        private readonly IStringLocalizer m_StringLocalizer;

        public CShopItemRemoveSell(ShopsPlugin shopsPlugin,
            ShopDbContext dbContext,
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_DbContext = dbContext;
            m_StringLocalizer = stringLocalizer;
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
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IdOrName = idOrName }]);
            }

            SellItem shop = await m_DbContext.SellItemShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_sell_shop_doesnt_exist", new { ItemName = asset.itemName, ItemId = asset.id }]);
            }

            m_DbContext.SellItemShops.Remove(shop);

            await m_DbContext.SaveChangesAsync();

            throw new UserFriendlyException(m_StringLocalizer["shops:success:item_sell_shop_removed", new { ItemName = asset.itemName, ItemId = asset.id }]);
        }
    }
}
