using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using SDG.Unturned;
using Shops.Commands.Actions;
using Shops.Shops;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("buy")]
    [CommandAlias("buyi")]
    [CommandAlias("ibuy")]
    [CommandAlias("buyitem")]
    [CommandAlias("itembuy")]
    [CommandDescription("Buys the specified item")]
    [CommandSyntax("<id or name> [amount]")]
    public class CBuyItem : CInteractAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;

        public CBuyItem(ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(stringLocalizer, serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
        }

        protected override async Task ExecuteInteractAsync(string idOrName, int amount)
        {
            m_ShopsPlugin.AssertCanBuyItems();

            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IdOrName = idOrName }]);
            }

            ShopBuyItem shop = await m_ShopsPlugin.GetBuyItemShop(asset.id);

            if (shop == null)
            {
                await User.PrintMessageAsync(m_StringLocalizer["shops:fail:item_not_for_sale", new { ItemName = asset.itemName, ItemId = asset.id } ]);
            }

            await shop.Interact(User, amount);
        }
    }
}
