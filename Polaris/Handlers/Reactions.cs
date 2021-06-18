using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Polaris.Models;

namespace Polaris.Handlers
{
    public class Reactions
    {

        public async Task ReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            var config = GuildConfig.Guilds[e.Guild.Id];

            if (config.rulesmessage is not null && e.Message == GuildConfig.Guilds[e.Guild.Id].rulesmessage)
            {
                var member = (DiscordMember) e.User;

                try
                {
                    await member.GrantRoleAsync(config.verifiedrole);
                }
                catch { }
            }
        }

        public async Task ReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs e)
        {
            var config = GuildConfig.Guilds[e.Guild.Id];
            
            if (config.rulesmessage is not null && e.Message == config.rulesmessage)
            {
                var member = (DiscordMember) e.User;

                try
                {
                    await member.RevokeRoleAsync(config.verifiedrole);
                }
                catch { }
            }
        }
    }
}