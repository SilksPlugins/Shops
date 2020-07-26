using Cysharp.Threading.Tasks;
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
        public UnturnedUser User => (UnturnedUser)Context.Actor;

        protected CInteractAction(IServiceProvider serviceProvider) : base(serviceProvider)
        {

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
                    throw new UserFriendlyException("Amount must be greater than zero.");
                }
            }

            await ExecuteInteractAsync(identifier, amount);
        }

        protected abstract Task ExecuteInteractAsync(string identifier, int amount);
    }
}
