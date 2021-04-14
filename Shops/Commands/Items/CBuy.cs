using OpenMod.API.Commands;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("buy", Priority = Priority.High)]
    [CommandAlias("ibuy")]
    [CommandAlias("itembuy")]
    [CommandAlias("buyi")]
    [CommandAlias("buyitem")]
    [CommandSyntax("<item> [amount]")]
    [CommandDescription("Buys the item from the shop.")]
    public class CBuy : ShopCommand
    {
        public CBuy(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var asset = await GetItemAsset(0);
            var amount = await GetAmount(1);

            var shop = await ShopManager.GetItemShop(asset.ItemAssetId);

            if (shop == null || !shop.CanBuy())
                throw new UserFriendlyException(
                    StringLocalizer["commands:errors:no_buyable_item_shop", new {ItemAsset = asset}]);

            var balance = await shop.Buy(GetPlayerUser(), amount);

            await PrintAsync(StringLocalizer["commands:success:item_bought",
                new
                {
                    Amount = amount,
                    ItemAsset = asset,
                    Price = shop.ShopData.BuyPrice * amount,
                    Balance = balance,
                    EconomyProvider.CurrencySymbol,
                    EconomyProvider.CurrencyName
                }]);
        }
    }
}
