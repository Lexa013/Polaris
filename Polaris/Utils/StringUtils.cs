using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace Polaris.Utils
{
    public static class StringUtils
    {
        public static string FeedFormatter(this string message, DiscordGuild guild, DiscordUser user)
        {
            var formatted = message
                .Replace("%SERVERNAME%", guild.Name)
                .Replace("%MEMBERCOUNT%", guild.MemberCount.ToString())
                .Replace("%USERNAME%", user.Username);

            return formatted;
        }
    }
}