using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace Polaris.Handlers
{
    public class Started
    {
        public Task OnReady(DiscordClient cl, ReadyEventArgs ev)
        {
            Console.WriteLine($"Logged as {cl.CurrentUser.Username}");

            return cl.UpdateStatusAsync(new DiscordActivity("with cats", ActivityType.Playing), UserStatus.Online);
        }
    }
}