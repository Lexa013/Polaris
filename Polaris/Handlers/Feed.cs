using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Polaris.Models;
using Polaris.Utils;

namespace Polaris.Handlers
{
    public class Feed
    {
        public async Task GuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs e)
        {
            var config = GuildConfig.Guilds[e.Guild.Id];
                
            if (config.welcomechannelid is not null)
            {
                var msg = config.welcomemessage.FeedFormatter(e.Guild, e.Member);
                
                await config.welcomechannelid.SendMessageAsync(msg);
            }
        }

        public async Task GuildMemberRemoved(DiscordClient sender, GuildMemberRemoveEventArgs e)
        {
            var config = GuildConfig.Guilds[e.Guild.Id];
                
            if (config.welcomechannelid is not null)
            {
                var msg = config.leavemessage.FeedFormatter(e.Guild, e.Member);
                
                await config.welcomechannelid.SendMessageAsync(msg);
            }
        }
    }
}