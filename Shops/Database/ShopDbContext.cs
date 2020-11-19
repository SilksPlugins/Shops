using Microsoft.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore;
using Shops.Database.Models;
using System;

namespace Shops.Database
{
    public class ShopDbContext : OpenModDbContext<ShopDbContext>
    {
        public DbSet<BuyItem> BuyItemShops { get; set; }

        public DbSet<SellItem> SellItemShops { get; set; }

        public DbSet<BuyVehicle> BuyVehicleShops { get; set; }

        public ShopDbContext(DbContextOptions<ShopDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
            
        }
    }
}
