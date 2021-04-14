using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("sell", Priority = Priority.High)]
    [CommandAlias("s")]
    [CommandSyntax("<item> <price>")]
    [CommandDescription("Adds the item to the shop to be sold.")]
    [CommandParent(typeof(CShopAdd))]
    public class CShopAddSell : ShopCommand
    {
        public CShopAddSell(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var asset = await GetItemAsset(0);
            var price = await GetPrice(1);

            await ShopManager.AddItemShopSellable(asset.ItemAssetId, price);

            await PrintAsync(
                StringLocalizer["commands:success:shop_added:sellable_item",
                    new
                    {
                        ItemAsset = asset,
                        Price = price,
                        EconomyProvider.CurrencySymbol,
                        EconomyProvider.CurrencyName
                    }]);
        }
    }
}