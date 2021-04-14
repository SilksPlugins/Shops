using OpenMod.API.Prioritization;
using OpenMod.Core.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Vehicles
{
    [Command("vshop", Priority = Priority.Normal)]
    [CommandAlias("vshops")]
    [CommandAlias("vehicleshop")]
    [CommandAlias("vehicleshops")]
    [CommandDescription("Manages vehicle shops.")]
    public class CVShop : Command
    {
        public CVShop(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
