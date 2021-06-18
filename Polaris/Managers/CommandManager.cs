using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Polaris.Managers
{
    public class CommandManager
    {
        public static async Task WaitForConfirmationAsync(CommandContext ctx, Action callback)
        {

            // Create buttonsz
            var btn_confirm = new DiscordButtonComponent(ButtonStyle.Success, "btn_confirm", "Confirm");
            var btn_cancel = new DiscordButtonComponent(ButtonStyle.Danger, "btn_cancel", "Cancel");
            
            var embed = new DiscordEmbedBuilder()
                .WithTitle("You're trying to run a special action")
                .WithDescription("Please confirm with the buttons bellow within 15 seconds")
                .WithColor(new DiscordColor("#27ae60"))
                .Build();

            var builder = new DiscordMessageBuilder()
                .WithEmbed(embed)
                .WithComponents(btn_confirm, btn_cancel);

            var msg = await ctx.RespondAsync(builder);

            var result = await msg.WaitForButtonAsync(ctx.User, TimeSpan.FromSeconds(15));

            if (!result.TimedOut)
            {
                if (result.Result.Id == "btn_confirm")
                    callback();
            }
            else
            {
                ctx.Channel.SendMessageAsync(":x: You didn't confirmed in time, your changes are not saved !");
            }
            
            // Anyway, delete message if something is completed, canceled or out of time
            await msg.DeleteAsync();
        }
    }
}