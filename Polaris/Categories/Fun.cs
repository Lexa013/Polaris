using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace Polaris.Categories
{
    public class Fun : BaseCommandModule
    {
        // public Joke Joke { private get; set;} // TODO Use Depency Injection
        
        [Command("lovetest"), Aliases("love", "lt"), RequireGuild]
        public async Task lovetest(CommandContext ctx, DiscordMember member)
        {
            Random random = new Random();

            await ctx.RespondAsync($"There is `{random.Next(0, 100)}%` of love between {ctx.Message.Author.Username} & {member.Username}");
        }

        #region Joke (Will be removed in a future update)
        /*
         
        [Command("joke")]
        [Aliases("blague")]
        public async Task JokeCMD(CommandContext ctx)
        {

            // Contact API
            var response = Unirest.get("https://www.blagues-api.fr/api/random")
                .header("Authorization", $"Bearer {Joke.token}")
                .asJson<string>()
                .Body;

            // Create joke object
            Joke joke = JsonConvert.DeserializeObject<Joke>(response);
            
            // Send embed
            await ctx.Channel.SendMessageAsync(
                embed: 
                new DiscordEmbedBuilder()
                    .WithTitle($":interrobang: {joke.joke}")
                    .AddField(":scroll: Answer", $"||{joke.answer}||", true)
                    .AddField(":satellite: Category", joke.Type().ToString())
                    .WithFooter($"Requested by {ctx.Message.Author.Username}", ctx.Message.Author.AvatarUrl)
                    .WithColor(new DiscordColor("#e74c3c"))
                    .Build()
            );
        }
        
        */
        #endregion
        
    }
}