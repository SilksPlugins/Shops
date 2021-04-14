using Shops.API.Items;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shops.Database.Models
{
    [Serializable]
    public class ItemShopModel : IItemShopData
    {
        [Key]
        public string ItemId { get; set; }

        public decimal? BuyPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public ItemShopModel()
        {
            ItemId = "";
            BuyPrice = null;
            SellPrice = null;
        }
    }
}
