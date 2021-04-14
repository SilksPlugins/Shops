using OpenMod.API.Plugins;
using OpenMod.EntityFrameworkCore.Extensions;
using Shops.Database;

namespace Shops
{
    public class ContainerConfigurator : IPluginContainerConfigurator
    {
        public void ConfigureContainer(IPluginServiceConfigurationContext context)
        {
            context.ContainerBuilder.AddEntityFrameworkCoreMySql();
            context.ContainerBuilder.AddDbContext<ShopsDbContext>();
        }
    }
}
