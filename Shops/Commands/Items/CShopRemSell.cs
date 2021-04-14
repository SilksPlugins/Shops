using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("sell")]
    [CommandAlias("s")]
    [CommandSyntax("<item>")]
    [CommandDescription("Removes the sellable item from the shop.")]
    [CommandParent(typeof(CShopRem))]
    public class CShopRemSell : ShopCommand
    {
        public CShopRemSell(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var asset = await GetItemAsset(0);

            if (await ShopManager.RemoveItemShopSellable(asset.ItemAssetId))
            {
                await PrintAsync(StringLocalizer["commands:success:shop_removed:sellable_item", new { ItemAsset = asset }]);
            }
            else
            {
                throw new UserFriendlyException(
                    StringLocalizer["commands:errors:no_sellable_item_shop", new {ItemAsset = asset}]);
            }
        }
    }
}