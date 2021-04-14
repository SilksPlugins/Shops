using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Items
{
    [Command("buy", Priority = Priority.Normal)]
    [CommandAlias("b")]
    [CommandSyntax("<item> <price>")]
    [CommandDescription("Adds the item to the shop to be bought.")]
    [CommandParent(typeof(CShopAdd))]
    public class CShopAddBuy : ShopCommand
    {
        public CShopAddBuy(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async Task OnExecuteAsync()
        {
            var asset = await GetItemAsset(0);
            var price = await GetPrice(1);

            await ShopManager.AddItemShopBuyable(asset.ItemAssetId, price);

            await PrintAsync(
                StringLocalizer["commands:success:shop_added:buyable_item",
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
