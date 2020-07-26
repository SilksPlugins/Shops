using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using System;

namespace Shops.Commands.Vehicles
{
    [Command("shopv")]
    [CommandAlias("vshop")]
    [CommandAlias("shopvehicle")]
    [CommandAlias("vehicleshop")]
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
