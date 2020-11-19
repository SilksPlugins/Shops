using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using SDG.Unturned;
using Shops.Database;
using Shops.Database.Models;
using System;
using System.Numerics;

namespace Shops.Commands.Items
{
    [Command("buy")]
    [CommandAlias("b")]
    [CommandSyntax("<id or name>")]
    [CommandParent(typeof(CShopItemRemove))]
    public class CShopItemRemoveBuy : UnturnedCommand
    {
        private readonly ShopsPlugin m_ShopsPlugin;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ShopDbContext m_DbContext;

        public CShopItemRemoveBuy(ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            ShopDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
            m_StringLocalizer = stringLocalizer;
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
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IdOrName = idOrName }]);
            }

            BuyItem shop = await m_DbContext.BuyItemShops.FindAsync((int)asset.id);

            if (shop == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_buy_shop_doesnt_exist", new { ItemName = asset.itemName, ItemId = asset.id }]);
            }

            m_DbContext.BuyItemShops.Remove(shop);

            await m_DbContext.SaveChangesAsync();

            throw new UserFriendlyException(m_StringLocalizer["shops:success:item_buy_shop_removed", new { ItemName = asset.itemName, ItemId = asset.id }]);
        }
    }
}
