using Microsoft.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore;
using Shops.Database.Models;
using System;

namespace Shops.Database
{
    public class ShopsDbContext : OpenModDbContext<ShopsDbContext>
    {
        public ShopsDbContext(
            DbContextOptions<ShopsDbContext> options,
            IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }

        public DbSet<ItemShopModel> ItemShops { get; set; } = null!;

        public DbSet<VehicleShopModel> VehicleShops { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemShopModel>()
                .HasKey(x => x.ItemId);

            modelBuilder.Entity<VehicleShopModel>()
                .HasKey(x => x.VehicleId);
        }
    }
}
