using Autofac;
using Microsoft.Extensions.Configuration;
using OpenMod.API.Plugins;
using OpenMod.EntityFrameworkCore.Extensions;
using Shops.Database;

namespace Shops
{
    public class ContainerConfigurator : IPluginContainerConfigurator
    {
        public void ConfigureContainer(
            ILifetimeScope parentLifetimeScope,
            IConfiguration configuration,
            ContainerBuilder containerBuilder)
        {

        }

        public void ConfigureContainer(IPluginServiceConfigurationContext context)
        {
            context.ContainerBuilder
                .AddEntityFrameworkCoreMySql()
                .AddDbContext<ShopDbContext>();
        }
    }
}
