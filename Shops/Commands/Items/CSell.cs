using OpenMod.API.Commands;
using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("sell", Priority = Priority.Normal)]
    [CommandAlias("isell")]
    [CommandAlias("itemsell")]
    [CommandAlias("selli")]
    [CommandAlias("sellitem")]
    [CommandSyntax("<item> [amount]")]
    [CommandDescription("Sells an item to the shop.")]
    public class CSell : ShopCommand
    {
        public CSell(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            AssertCanSellItems();

            var asset = await GetItemAsset(0);
            var amount = await GetAmount(1);

            var shop = await ShopManager.GetItemShop(asset.ItemAssetId);

            if (shop == null || !shop.CanSell())
                throw new UserFriendlyException(
                    StringLocalizer["commands:errors:no_sellable_item_shop", new { ItemAsset = asset }]);

            var balance = await shop.Sell(GetPlayerUser(), amount);

            await PrintAsync(StringLocalizer["commands:success:item_sold",
                new
                {
                    Amount = amount,
                    ItemAsset = asset,
                    Price = shop.ShopData.SellPrice * amount,
                    Balance = balance,
                    EconomyProvider.CurrencySymbol,
                    EconomyProvider.CurrencyName
                }]);
        }
    }
}
