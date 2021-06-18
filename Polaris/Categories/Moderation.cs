using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Polaris.Managers;

namespace Polaris.Categories
{
    public class Moderation : BaseCommandModule
    {
        [Command("clear"), Description("Clear message in the current channel")]
        [RequirePermissions(Permissions.ManageMessages), RequireGuild]
        public async Task Clear(CommandContext ctx, [Description("Amount of messages to delete (1-100)")] int amount)
        {
            if (amount <= 0 || amount > 100)
            {
                await ctx.Channel.SendMessageAsync(
                    new DiscordEmbedBuilder()
                        .WithTitle($":warning: Wrong arguments")
                        .WithDescription("The number of message must be at least 1 and 100 maximum")
                        .WithColor(DiscordColor.IndianRed)
                        .Build()
                );
                
                return;
            }
            
            var _messages = await ctx.Channel.GetMessagesAsync(amount + 1);


            await ctx.Channel.DeleteMessagesAsync(_messages, $"Clear - {ctx.Message.Author.Username}");

        }
    }
}