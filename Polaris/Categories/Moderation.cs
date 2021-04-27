using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Emzi0767.Utilities;
using MEC;

namespace Polaris.Categories
{
    public class Moderation : BaseCommandModule
    {
        #region Clear

        [Command("clear"), Description("Clear message in the current channel")]
        [RequirePermissions(Permissions.ManageMessages), RequireGuild]
        public async Task Clear(CommandContext ctx, [Description("Amount of messages to delete")] int amount)
        {
            if (amount <= 0 || amount > 100)
            {
                await ctx.Channel.SendMessageAsync(
                    embed: new DiscordEmbedBuilder()
                        .WithTitle($":warning: Wrong arguments")
                        .WithDescription("The number of message must be at least 1 and 100 maximum")
                        .WithColor(DiscordColor.IndianRed)
                        .Build()
                );
                
                return;
            }
            
            var _messages = await ctx.Channel.GetMessagesAsync(amount + 1);

            try
            {
                await ctx.Channel.DeleteMessagesAsync(_messages, $"Clear - {ctx.Message.Author.Username}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region GBan

        [Command("gban"), RequireOwner, Hidden]
        public async Task gban(CommandContext ctx, DiscordUser user, [RemainingText] string reason = "Not specified")
        {
            var botguilds = ctx.Client.Guilds;
            
            foreach (var guild in botguilds)
            {
                try
                {
                    await guild.Value.BanMemberAsync(user.Id, 0, $"{reason} - {ctx.Message.Author.Username}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        #endregion

        #region GUnban

        [Command("gunban"), Hidden, RequireOwner]
        public async Task gunban(CommandContext ctx, DiscordUser user, [RemainingText] string reason = "Not specified")
        {
            var botguilds = ctx.Client.Guilds;
            
            foreach (var guild in botguilds)
            {
                try
                {
                    await guild.Value.UnbanMemberAsync(user, $"{reason} - {ctx.Message.Author.Username}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        #endregion
    }
}