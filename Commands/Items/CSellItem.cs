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
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_ShopsPlugin = shopsPlugin;
        }

        protected override async Task ExecuteInteractAsync(string idOrName, int amount)
        {
            Asset asset = m_ShopsPlugin.GetAsset(EAssetType.ITEM, idOrName);

            if (asset == null)
            {
                throw new UserFriendlyException("Item not found");
            }

            ShopSellItem shop = await m_ShopsPlugin.GetSellItemShop(asset.id);

            if (shop == null)
            {
                await User.PrintMessageAsync("Cannot sell this item");
            }

            await shop.Interact(User, amount);
        }
    }
}
