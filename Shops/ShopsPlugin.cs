using Autofac;
using OpenMod.API.Plugins;
using OpenMod.Core.Plugins;
using OpenMod.Extensions.Economy.Abstractions;
using Shops.Database;
using System;
using System.Threading.Tasks;

[assembly: PluginMetadata("Shops", DisplayName = "Shops",
    Author = "SilK", Website = "https://github.com/IAmSilK/Shops")]
namespace Shops
{
    public class ShopsPlugin : OpenModUniversalPlugin
    {
        private readonly ShopsDbContext _dbContext;

        public ShopsPlugin(
            ShopsDbContext dbContext,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _dbContext = dbContext;
        }

        protected override async Task OnLoadAsync()
        {
            LifetimeScope.Resolve<IEconomyProvider>();

            await _dbContext.OpenModMigrateAsync();

        }

        protected override Task OnUnloadAsync() => Task.CompletedTask;
    }
}
