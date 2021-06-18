using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Polaris.Managers;

namespace Polaris.Categories
{
    public class Fun : BaseCommandModule
    {
        [Command("lovetest"), Aliases("love"), RequireGuild, Description("Calculate love between two members")]
        public async Task LoveTest(CommandContext ctx, [Description("First user")] DiscordMember member1,
            [Description("Second user")] DiscordMember member2)
        {
            Random random = new Random();

            await ctx.RespondAsync($"There is `{random.Next(0, 100)}%` of love between {member1.Username} & {member2.Username}");
        }
    }
}