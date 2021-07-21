using OpenMod.API.Plugins;
using Shops.Database;
using OpenMod.EntityFrameworkCore.MySql.Extensions;

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
