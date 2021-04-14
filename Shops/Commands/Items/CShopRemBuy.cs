using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("buy")]
    [CommandAlias("b")]
    [CommandSyntax("<item>")]
    [CommandDescription("Removes the buyable item from the shop.")]
    [CommandParent(typeof(CShopRem))]
    public class CShopRemBuy : ShopCommand
    {
        public CShopRemBuy(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var asset = await GetItemAsset(0);

            if (await ShopManager.RemoveItemShopBuyable(asset.ItemAssetId))
            {
                await PrintAsync(
                    StringLocalizer["commands:success:shop_removed:buyable_item", new {ItemAsset = asset}]);
            }
            else
            {
                throw new UserFriendlyException(
                    StringLocalizer["commands:errors:no_buyable_item_shop", new {ItemAsset = asset}]);
            }
        }
    }
}