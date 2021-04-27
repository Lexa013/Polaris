﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Polaris.Classes;

namespace Polaris.Categories
{
    [Group("config"), Aliases("configuration")]
    public class Configuration : BaseCommandModule
    {
        public Config Config { private get; set; }
        
        [GroupCommand]
        [RequireGuild]
        [RequirePermissions(Permissions.Administrator)]
        public async Task ConfigStatus(CommandContext ctx)
        {
            var _guild = Config.Guilds[ctx.Guild.Id];
            
            Dictionary<string, string> _details = new()
            {
                {":scroll: Rules message ID", _guild.rulesmessage == 0 ? ":x:" : $":white_check_mark: ({_guild.rulesmessage})"},
                {":airplane: Welcome channel", _guild.welcomechannelid == 0 ? ":x:" : $":white_check_mark: (<#{_guild.welcomechannelid}>)"}
            };

            var embed = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor("#8e44ad"))
                .WithTitle(":gear: Bot configuration")
                .WithFooter($"Type {Config.prefix[1]} config help for help", ctx.Client.CurrentUser.AvatarUrl);

            foreach (var detail in _details)    
            {
                embed.AddField(detail.Key, (string) detail.Value, true);
            }

            await ctx.RespondAsync(embed: embed);
        }
        
        [Command("rules"), RequireGuild, RequirePermissions(Permissions.Administrator)]
        public async Task RulesID(CommandContext ctx, DiscordMessage message)
        {
            var newconfig = Config.Guilds[ctx.Guild.Id];

            if (message.Channel.Guild.Equals(ctx.Guild))
            {
                newconfig.rulesmessage = message.Id; // TODO If arg(message) equals -1 then newconfig.rulesmessage = -1
                Config.UpdateGuildConfig(ctx.Guild, newconfig);
                return;
                
            }
            
            await ctx.RespondAsync(":x: Cannot configure a message wich isn't in this guild");
        }
        
        [Command("welcome"), RequireGuild]
        public async Task WelcomeChannel(CommandContext ctx, DiscordChannel channel)
        {
            var newconfig = Config.Guilds[ctx.Guild.Id];

            if (channel.Guild.Equals(ctx.Guild))
            {
                newconfig.welcomechannelid = channel.Id;
                Config.UpdateGuildConfig(ctx.Guild, newconfig);
                await ctx.RespondAsync("Success");
                return;
            }
            
            await ctx.RespondAsync(":x: Cannot configure a channel wich isn't in this guild");
        }

        [Command("inspect"), RequireOwner, Hidden]
        public async Task Inspect(CommandContext ctx, DiscordGuild guild = null)
        {
            guild = guild == null ? guild = ctx.Guild : guild;
            var config = Newtonsoft.Json.JsonConvert.SerializeObject(Config.Guilds[guild.Id]);

            ctx.RespondAsync($"```\n{config}\n```");
        }

        // TODO Reset command
    }
}