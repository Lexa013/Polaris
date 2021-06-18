using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Polaris.Categories
{
    public class Utility : BaseCommandModule
    {
        [Command("say"), Description("Say a message in a channel"), RequireGuild, RequirePermissionsAttribute(Permissions.ManageMessages)]
        public async Task say(CommandContext ctx, [Description("The channel")] DiscordChannel channel, [RemainingText, Description("Message")] string msg)
        {
            try
            {
                await channel.SendMessageAsync(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);                   
                throw;
            }
        }
    }
}