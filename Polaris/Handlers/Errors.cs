using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using Polaris.Managers;
using Polaris.Models;
using Renci.SshNet.Messages;

namespace Polaris.Handlers
{
    public class Errors: BaseCommandModule
    {
        private Config _config;
        
        public async Task OnErrored(CommandsNextExtension cn, CommandErrorEventArgs ev)
        {
            Task task = ev.Exception switch
            {
                ChecksFailedException f => FindFailedMessage(ev, cn.Client, f),

                ArgumentException
                    {Message: "Could not find a suitable overload for the command."} => ShowHelp(cn.Client,
                    ev.Command.QualifiedName, ev.Context),

                InvalidOperationException
                    {Message: "No matching subcommands were found, and this group is not executable."} => ShowHelp(
                    cn.Client, ev.Command.QualifiedName, ev.Context),
                
                BadRequestException e => FindBadRequestMessage(ev, cn.Client, e)
            };

            await task;
        }

        private async Task FindBadRequestMessage(CommandErrorEventArgs ev, DiscordClient c, BadRequestException ex)
        {
            var msg = ex switch
            {
                {Message: "Bad request: 400"} => "The request was improperly formatted, or the server couldn't understand it",
                {Message: "Bad request: 401"} => "Unauthorized from Discord API",
                {Message: "Bad request: 403"} => "The Discord API returned a forbidden exception",
                {Message: "Bad request: 404"} => "Not found",
                {Message: "Bad request: 405"} => "The HTTP method used is not valid for the location specified",
                {Message: "Bad request: 429"} => "Too many requests, the bot is being rate limited",
                {Message: "Bad request: 502"} => "Discord gateway error",
                _ => null
            };

            var embed = new DiscordEmbedBuilder()
                .WithTitle(":warning: Discord API error")
                .WithColor(new DiscordColor(255, 204, 77));

            if (msg is not null)
            {
                embed.Description = msg;
            }
            else
            {
                embed.Description = $"An unhandled discord API response happened: ```{ev.Exception}```";
            }

            ev.Context.RespondAsync(embed.Build());

        }
        
        private async Task FindFailedMessage(CommandErrorEventArgs ev, DiscordClient c, ChecksFailedException f)
        {
            string botowner = _config.botowner;

            foreach (var check in f.FailedChecks)
            {
                Console.WriteLine($"Avoiding {check} exception");
                string? message = check switch
                {
                    RequireOwnerAttribute => ":no_entry: This command is restricted to the bot owner !",
                    RequireNsfwAttribute => ":lipstick: You need nsfw permissions !",
                    CooldownAttribute cd =>
                        $":timer: This command is on cooldown, you'll be able to run it in {cd.GetRemainingCooldown(ev.Context).Seconds} seconds.",
                    RequireUserPermissionsAttribute up =>
                        $":x: Sorry, you need the permission `{up.Permissions.ToPermissionString()}` to execute this command !",
                    RequireDirectMessageAttribute => ":warning: This command is restricted to DM only !",
                    RequireGuildAttribute => ":warning: This command is restricted to guild only !",
                    _ => null
                };
                
                if (message is not null)
                {
                    await ev.Context.RespondAsync(message);
                    return;
                }
            }
        }

        private async Task ShowHelp(DiscordClient client, string commandName, CommandContext baseContext)
        {
            CommandsNextExtension? cNext = client.GetCommandsNext();
            Command? cmd = cNext.RegisteredCommands["help"];
            CommandContext? ctx = cNext.CreateContext(baseContext.Message, null, cmd, commandName); 
            await cNext.ExecuteCommandAsync(ctx);
        }

        public Errors(Config config)
        {
            _config = config;
        }
    }
}