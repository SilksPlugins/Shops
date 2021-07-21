using Microsoft.EntityFrameworkCore;
using OpenMod.EntityFrameworkCore;
using Shops.Database.Models;
using System;
using System.Threading.Tasks;
using OpenMod.EntityFrameworkCore.Configurator;

namespace Shops.Database
{
    public class ShopsDbContext : OpenModDbContext<ShopsDbContext>
    {
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

        await m_DbContext.Database.MigrateAsync();
    }
}
