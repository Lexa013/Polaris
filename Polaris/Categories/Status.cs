using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MineStatLib;

namespace Polaris.Categories
{
    [Group("status")]
    public class Status : BaseCommandModule
    {
        [Command("minecraft")]
        [Aliases("mc")]
        [Description("Get the status of a minecraft server")]
        public async Task Minecraft(CommandContext ctx, [Description("IP adress")] string ip, [Description("Port")] int port=25565)
        {
            MineStat m = new MineStat(ip, (ushort) port);

            if (!m.ServerUp)
            {
                await ctx.Channel.SendMessageAsync(
                    new DiscordEmbedBuilder()
                        .WithTitle(":warning: Unable to reach the server")
                        .WithColor(new DiscordColor("#e74c3c"))
                        .WithFooter($"Requested by {ctx.Message.Author.Username}", ctx.Message.Author.AvatarUrl)
                        .Build());
                return;
            }

            Dictionary<string, string> _details = new()
                {
                    {":satellite: Address", $"`{m.Address}:{m.Port}`"},
                    {":family_man_girl_boy: Players", m.CurrentPlayers+ "/" +m.MaximumPlayers},
                    {":o: Server version", m.Version},
                };
            
            DiscordEmbedBuilder status_embed = new();
            status_embed
                .WithTitle("Server status")
                .WithColor(new DiscordColor("#16a085"))
                .WithFooter($"Requested by {ctx.Message.Author.Username}", ctx.Message.Author.AvatarUrl);

            foreach (var detail in _details)
            {
                status_embed.AddField(detail.Key, detail.Value, true);
            }

            await ctx.Channel.SendMessageAsync(embed:status_embed);
        }
    }
}