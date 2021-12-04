using Shops.API.Vehicles;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shops.Database.Models
{
    [Serializable]
    public class VehicleShopModel : IVehicleShopData
    {
        [Key]
        [MaxLength(128)]
        public string VehicleId { get; set; }

        public decimal BuyPrice { get; set; }

        public VehicleShopModel()
        {
            VehicleId = "";
            BuyPrice = 0;
        }
    }
}
