using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using System.Threading.Tasks;

namespace Shops.Commands.Actions
{
    [CommandActor(typeof(UnturnedUser))]
    public abstract class CInteractAction : UnturnedCommand
    {
        protected readonly IStringLocalizer m_StringLocalizer;

        public UnturnedUser User => (UnturnedUser)Context.Actor;

        protected CInteractAction(IStringLocalizer stringLocalizer,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_StringLocalizer = stringLocalizer;
        }

        protected async override UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Length != 1 && Context.Parameters.Length != 2)
            {
                throw new CommandWrongUsageException(Context);
            }

            string identifier = await Context.Parameters.GetAsync<string>(0);
            int amount = 1;

            if (Context.Parameters.Length == 2)
            {
                amount = await Context.Parameters.GetAsync<int>(1);

                if (amount <= 0)
                {
                    throw new UserFriendlyException(m_StringLocalizer["shops:fail:amount_above_zero"]);
                }
            }

            await ExecuteInteractAsync(identifier, amount);
        }

        protected abstract Task ExecuteInteractAsync(string identifier, int amount);
    }
}
