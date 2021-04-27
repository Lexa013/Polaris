using System.Reflection;
using System.Threading.Tasks;
using DatabaseWrapper.Core;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Polaris.Classes;
using Polaris.Handlers;

namespace Polaris
{
    class Polaris
    {
        public static async Task Main(string[] args)
        {
            var database = new Database(DbTypes.Mysql, "127.0.0.1", 3306, "lexa", "lexa", "polaris");

            var discord = new DiscordShardedClient(new DiscordConfiguration
            {
                Token = args[0],
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            });
            
            // Instanciate classes

            var errors = new Errors();
            var ready = new Started();
            var config = new Config(database, args);
            var reactions = new Reactions(config);
            var actions = new Actions(database, config);

            var services = new ServiceCollection()
                .AddSingleton(config)
                .AddSingleton(database)
                .BuildServiceProvider();
                
            var commands = await discord.UseCommandsNextAsync(new CommandsNextConfiguration
            {
                StringPrefixes = new[] {args[1]},
                EnableDefaultHelp = true,
                Services = services

            });

            foreach (var c in commands.Values)
            {
                c.RegisterCommands(Assembly.GetExecutingAssembly());

                c.CommandErrored += errors.OnErrored;
                
                // Bot hanlers
                discord.Ready += ready.OnReady;
                discord.GuildDownloadCompleted += actions.GuildDownloadCompleted;
                discord.GuildCreated += actions.GuildJoined;
                discord.GuildDeleted += actions.GuildDeleted;
                
                // Reaction handlers
                discord.MessageReactionAdded += reactions.ReactionAdded;
                discord.MessageReactionRemoved += reactions.ReactionRemoved;
            }

            await discord.StartAsync();
            await Task.Delay(-1);
        }
    }
}