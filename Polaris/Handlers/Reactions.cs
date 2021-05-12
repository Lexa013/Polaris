using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Polaris.Classes;

namespace Polaris.Handlers
{
    public class Reactions
    {

        public async Task ReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            if (GuildConfig.Guilds[e.Guild.Id].rulesmessage != 0 && e.Message.Id == GuildConfig.Guilds[e.Guild.Id].rulesmessage)
            {
                var member = (DiscordMember) e.User;
                try
                {
                    await member.GrantRoleAsync(e.Guild.GetRole(834433733578719312));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        public async Task ReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs e)
        {
            if (GuildConfig.Guilds[e.Guild.Id].rulesmessage != 0 && e.Message.Id == GuildConfig.Guilds[e.Guild.Id].rulesmessage)
            {
                var member = (DiscordMember) e.User;
                try
                {
                    await member.RevokeRoleAsync(e.Guild.GetRole(834433733578719312));
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        public Reactions(Config config)
        {
        }
    }
}