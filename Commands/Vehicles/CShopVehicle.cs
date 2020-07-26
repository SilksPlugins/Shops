using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using System;

namespace Shops.Commands.Vehicles
{
    [Command("vshop")]
    [CommandAlias("shopv")]
    [CommandAlias("vehicleshop")]
    [CommandAlias("shopvehicle")]
    [CommandDescription("Manages vehicle shops")]
    [CommandSyntax("<[a]dd/[r]emove/[c]hange>")]
    public class CShopVehicle : UnturnedCommand
    {
        public CShopVehicle(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        protected override UniTask OnExecuteAsync()
        {
            throw new CommandWrongUsageException(Context);
        }
    }
}
