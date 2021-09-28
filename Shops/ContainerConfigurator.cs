using OpenMod.API.Plugins;
using OpenMod.EntityFrameworkCore.MySql.Extensions;
using Shops.Database;

namespace Shops
{
    public class ContainerConfigurator : IPluginContainerConfigurator
    {
        public void ConfigureContainer(IPluginServiceConfigurationContext context)
        {
            context.ContainerBuilder.AddMySqlDbContext<ShopsDbContext>();
        }
    }
}
