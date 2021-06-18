using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Polaris.Models;

namespace Polaris.Categories
{
    [Group("Bot"), Description("Commands related to the bot"), Hidden]
    public class Bot : BaseCommandModule
    {
        [Command("restart"), Aliases("reboot"), Description("Restart the bot"), RequireOwner]
        public async Task Restart(CommandContext ctx)
        {
            await ctx.RespondAsync(":arrows_counterclockwise: Rebooting...");
            await ctx.Client.DisconnectAsync();
            await Task.Delay(3000);
            GuildConfig.Guilds.Clear();
            await ctx.Client.ConnectAsync();
            await ctx.RespondAsync(":pause_button: Bot rebooted");
        }
    }
}