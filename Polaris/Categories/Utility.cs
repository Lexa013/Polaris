using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace Polaris.Categories
{
    public class Utility : BaseCommandModule
    {
        [Command("say"), RequireGuild, RequireUserPermissions(Permissions.ManageMessages),]
        [RequirePermissions(Permissions.Administrator)]
        public async Task say(CommandContext ctx, DiscordChannel channel, [RemainingText] string msg)
        {
            await channel.SendMessageAsync(msg);
        }
    }
}