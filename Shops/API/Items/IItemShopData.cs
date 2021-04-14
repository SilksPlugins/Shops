namespace Shops.API.Items
{
    public interface IItemShopData
    {
        public string ItemId { get; }

        public decimal? BuyPrice { get; }

        public decimal? SellPrice { get; }
    }
}
