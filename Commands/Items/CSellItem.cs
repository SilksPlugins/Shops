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
    [Command("sell")]
    [CommandAlias("selli")]
    [CommandAlias("isell")]
    [CommandAlias("sellitem")]
    [CommandAlias("itemsell")]
    [CommandDescription("Sells the specified item")]
    [CommandSyntax("<id or name> [amount]")]
    public class CSellItem : CInteractAction
    {
        private readonly ShopsPlugin m_ShopsPlugin;

        public CSellItem(ShopsPlugin shopsPlugin,
            IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(stringLocalizer, serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
        }

        protected override async Task ExecuteInteractAsync(string idOrName, int amount)
        {
            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:item_not_found", new { IDOrName = idOrName }]);
            }

            ShopSellItem shop = await m_ShopsPlugin.GetSellItemShop(asset.id);

            if (shop == null)
            {
                await User.PrintMessageAsync(m_StringLocalizer["shops:fail:item_not_for_sell", new { ItemName = asset.itemName, ItemID = asset.id }]);
            }

            await shop.Interact(User, amount);
        }
    }
}
