using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace Polaris.Handlers
{
    public class Errors: BaseCommandModule
    {
        
        public async Task OnErrored(CommandsNextExtension cn, CommandErrorEventArgs ev)
        {
            await Task.Run(async () =>
            {

                switch (ev.Exception.Message)
                {
                    case "Could not find a suitable overload for the command.":
                        await ev.Context.Channel.SendMessageAsync(
                            embed: new DiscordEmbedBuilder()
                                .WithTitle(":warning: Wrong arguments")
                                .WithDescription("Missing or invalids arguments")
                                .WithColor(DiscordColor.IndianRed)
                                .Build()
                        );
                        break;
                    
                    case "One or more pre-execution checks failed.":
                        await ev.Context.Channel.SendMessageAsync(
                            embed: new DiscordEmbedBuilder()
                                .WithTitle(":no_entry: Missing permissions")
                                .WithDescription($"You don't have enough permission to execute the command `{ev.Command.Name}`")
                                .WithColor(DiscordColor.IndianRed)
                                .Build()
                        );
                        break;

                    case "Specified command was not found.":
                        await ev.Context.Channel.SendMessageAsync(
                            embed: new DiscordEmbedBuilder()
                                .WithTitle(":interrobang: Unknown command")
                                .WithDescription($"The specified command don't seems to exist")
                                .WithColor(DiscordColor.IndianRed)
                                .Build()
                        );
                        break;
                    
                    case "No matching subcommands were found, and this group is not executable.":
                        await ev.Context.Channel.SendMessageAsync(
                            embed: new DiscordEmbedBuilder()
                                .WithTitle(":interrobang: Unknown command")
                                .WithDescription($"The specified command don't seems to exist in this category")
                                .WithColor(DiscordColor.IndianRed)
                                .Build()
                        );
                        break;
                    
                    default:
                        await ev.Context.Channel.SendMessageAsync(
                            embed: new DiscordEmbedBuilder()
                                .WithTitle(":warning: An error has occured")
                                .WithFooter("If you think there is a problem please contact the bot owner: Lexa#3625", cn.Client.CurrentUser.AvatarUrl)
                                .WithDescription(ev.Exception.Message)
                                .WithColor(DiscordColor.IndianRed)
                                .Build()
                        );
                        break;
                }
            });
        }
    }
}