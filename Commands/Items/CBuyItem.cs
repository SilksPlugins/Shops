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
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
        }

        protected override async Task ExecuteInteractAsync(string idOrName, int amount)
        {
            ItemAsset asset = (ItemAsset)m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Item not found");
            }

            ShopBuyItem shop = await m_ShopsPlugin.GetBuyItemShop(asset.id);

            if (shop == null)
            {
                await User.PrintMessageAsync("Item not for sale");
            }

            await shop.Interact(User, amount);
        }
    }
}
