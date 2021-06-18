using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Polaris.Models;
using Polaris.Managers;

namespace Polaris.Categories
{
    [Group("config"), Aliases("configuration"), Description("Module related to guild configurations"), RequireUserPermissions(Permissions.Administrator)]
    public class Configuration : BaseCommandModule
    {
        public GuildManager GuildManager { private get; set; }
        public Config Config { private get; set; }
        
        [GroupCommand, RequireUserPermissions(Permissions.Administrator), Description("Show informations about the guild config")]
        public async Task ConfigStatus(CommandContext ctx)
        {
            if (ctx.Guild == null)
            {
                ctx.RespondAsync(":warning: Guild command only !"); 
                return;
            }

            var _guild = GuildConfig.Guilds[ctx.Guild.Id];

            Dictionary<string, string> _details = new()
            {
                {":scroll: Rules message ID", _guild.rulesmessage is null ? ":x:" : $":white_check_mark: ({_guild.rulesmessage.Id})"},
                {":airplane: Feed channel", _guild.welcomechannelid is null ? ":x:" : $":white_check_mark: (<#{_guild.welcomechannelid.Id}>)"},
                {":inbox_tray: Welcome message", _guild.welcomemessage is null ? ":x:" : $"```{_guild.welcomemessage}```"},
                {":outbox_tray: Leave message", _guild.leavemessage is null ? ":x:" : $"```{_guild.leavemessage}```"},
                {":cyclone: Verified roles", _guild.verifiedrole is null ? ":x:" : $"<@&{_guild.verifiedrole.Id}>"}

            };

            var embed = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor("#8e44ad"))
                .WithTitle(":gear: Bot configuration")
                .WithFooter($"Type {Config.Prefix}config help for help", ctx.Client.CurrentUser.AvatarUrl);

            foreach (var detail in _details)    
            {
                embed.AddField(detail.Key, detail.Value, true);
            }

            await ctx.RespondAsync(embed);
        }
        
        [Command("rules"), RequireGuild, RequireUserPermissions(Permissions.ManageGuild ), Description("Configure the rules message")]
        public async Task RulesID(CommandContext ctx, [Description("The rules message link")] DiscordMessage message)
        {
            var newconfig = GuildConfig.Guilds[ctx.Guild.Id];

            if (message.Channel.Guild.Equals(ctx.Guild))
            {
                newconfig.rulesmessage = message;
                await CommandManager.WaitForConfirmationAsync(ctx, () =>
                {
                    GuildManager.UpdateGuildConfig(ctx.Guild, newconfig);
                });
                return;
            }
            
            await ctx.RespondAsync(":x: Cannot configure a message wich isn't in this guild");
        }
        
        [Command("verifiedrole"), RequireGuild, RequireUserPermissions(Permissions.ManageGuild ), Description("Configure the rules message")]
        public async Task VerifiedRole(CommandContext ctx, [Description("Configure the role message")] DiscordRole role)
        {
            var newconfig = GuildConfig.Guilds[ctx.Guild.Id];
            
            newconfig.verifiedrole = role;
            await CommandManager.WaitForConfirmationAsync(ctx, () =>
            {
                GuildManager.UpdateGuildConfig(ctx.Guild, newconfig);
            });
        }
        
        [Command("feedchannel"), RequireGuild, RequireUserPermissions(Permissions.ManageGuild), Description("Configure the feed chanel")]
        public async Task WelcomeChannel(CommandContext ctx, [Description("Feed channel")] DiscordChannel channel)
        {
            var newconfig = GuildConfig.Guilds[ctx.Guild.Id];

            if (channel.Guild.Equals(ctx.Guild))
            {
                newconfig.welcomechannelid = channel;
                await CommandManager.WaitForConfirmationAsync(ctx, () =>
                {
                    GuildManager.UpdateGuildConfig(ctx.Guild, newconfig);
                });
                return;
            }
            
            await ctx.RespondAsync(":x: Cannot configure a channel wich isn't in this guild");
        }
        
        [Command("welcomemessage"), RequireGuild, RequireUserPermissions(Permissions.ManageGuild),
         Description("Configure the join message")]
        public async Task WelcomeMessage(CommandContext ctx, [RemainingText, Description("Message displayed when someone join")] string message)
        {
            var newconfig = GuildConfig.Guilds[ctx.Guild.Id];
            
            newconfig.welcomemessage = message;
            await CommandManager.WaitForConfirmationAsync(ctx, () =>
            {
                GuildManager.UpdateGuildConfig(ctx.Guild, newconfig);
            });
        }
        
        [Command("leavemessage"), RequireGuild, RequireUserPermissions(Permissions.ManageGuild), Description("Configure the leave message")]
        public async Task LeaveMessage(CommandContext ctx,[RemainingText, Description("Message displayed when someone leave")] string message)
        {
            var newconfig = GuildConfig.Guilds[ctx.Guild.Id];
            
            newconfig.leavemessage = message;
            await CommandManager.WaitForConfirmationAsync(ctx, () =>
            {
                GuildManager.UpdateGuildConfig(ctx.Guild, newconfig);
            });
        }

        [Command("inspect"), RequireOwner, RequireDirectMessage]
        public async Task Inspect(CommandContext ctx, DiscordGuild guild = null)
        {
            guild = guild == null ? guild = ctx.Guild : guild;
            var config = Newtonsoft.Json.JsonConvert.SerializeObject(GuildConfig.Guilds[guild.Id]);

            ctx.RespondAsync($"```\n{config}\n```");
        }
    }
}