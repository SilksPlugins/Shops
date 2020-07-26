using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Extensions.Economy.Abstractions;
using OpenMod.Unturned.Commands;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Actions
{
    public abstract class CShopAction : UnturnedCommand
    {
        protected readonly IStringLocalizer m_StringLocalizer;

        protected CShopAction(IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
        }

        protected async override UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Length != 2)
            {
                throw new CommandWrongUsageException(Context);
            }

            string identifier = await Context.Parameters.GetAsync<string>(0);
            int price = await Context.Parameters.GetAsync<int>(1);

            if (price <= 0)
            {
                throw new UserFriendlyException(m_StringLocalizer["shops:fail:price_above_zero"]);
            }

            await ExecuteShopUpdateAsync(identifier, price);
        }

        protected abstract Task ExecuteShopUpdateAsync(string identifier, decimal price);
    }
}
