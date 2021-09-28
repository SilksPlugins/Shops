using Microsoft.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore.Configurator;
using Shops.Database.Models;
using System;

namespace Shops.Database
{
    public class ShopsDbContext : OpenModDbContext<ShopsDbContext>
    {
        public ShopsDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ShopsDbContext(IDbContextConfigurator configurator, IServiceProvider serviceProvider) : base(configurator, serviceProvider)
        {
        }

        public DbSet<ItemShopModel> ItemShops => Set<ItemShopModel>();

        public DbSet<VehicleShopModel> VehicleShops => Set<VehicleShopModel>();

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
