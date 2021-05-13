using System;
using System.Reflection;
using System.Threading.Tasks;
using DatabaseWrapper.Core;
using DatabaseWrapper.Mysql;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polaris.Classes;
using Polaris.Handlers;
using Polaris.Managers;

namespace Polaris
{
    class Polaris
    {
        public static async Task Main(string[] args)
        {
            // Create config
            var configFile = System.IO.File.ReadAllText($@"{args[0]}");
            var Config = JsonConvert.DeserializeObject<Config>(configFile);

            var discord = new DiscordShardedClient(new DiscordConfiguration
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            });
            
            // Instanciate managers

            var guildManager = new GuildManager(Config);
            
            // Instanciate classes

            var errors = new Errors();
            var ready = new Started();
            var reactions = new Reactions();
            var actions = new Actions(guildManager);
            
            var services = new ServiceCollection()
                .AddSingleton<GuildManager>()
                .AddSingleton(Config)
                .BuildServiceProvider();
                
            var commands = await discord.UseCommandsNextAsync(new CommandsNextConfiguration
            {
                StringPrefixes = new[] {Config.Prefix},
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