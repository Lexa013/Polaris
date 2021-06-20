using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polaris.Models;
using Polaris.Handlers;
using Polaris.Managers;

namespace Polaris
{
    class Polaris
    {
        public static async Task Main(string[] args)
        {
            // Create config
            var configFile = await System.IO.File.ReadAllTextAsync($@"{args[0]}");
            var config = JsonConvert.DeserializeObject<Config>(configFile);
            config.Init();

            var discord = new DiscordShardedClient(new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All,
            });

            var guildManager = new GuildManager(config);

            // Instanciate classes

            var feed = new Feed();
            var errors = new Errors(config);
            var ready = new Started();
            var reactions = new Reactions();
            var actions = new Actions(guildManager);
            
            var services = new ServiceCollection()
                .AddSingleton(guildManager)
                .AddSingleton(config)
                .BuildServiceProvider();
                
            var commands = await discord.UseCommandsNextAsync(new CommandsNextConfiguration
            {
                StringPrefixes = new[] {config.Prefix},
                EnableDefaultHelp = true,
                Services = services

            });

            foreach (var c in commands.Values)
            {
                c.RegisterCommands(Assembly.GetExecutingAssembly());
                c.CommandErrored += errors.OnErrored;
            }
            
            RegisterEvents();

            void RegisterEvents()
            {
                // Bot hanlers
                discord.Ready += ready.OnReady;
                discord.GuildDownloadCompleted += actions.GuildDownloadCompleted;
                discord.GuildCreated += actions.GuildJoined;
                discord.GuildDeleted += actions.GuildDeleted;

                // Reaction handlers
                discord.MessageReactionAdded += reactions.ReactionAdded;
                discord.MessageReactionRemoved += reactions.ReactionRemoved;

                // Feed handlers
                discord.GuildMemberAdded += feed.GuildMemberAdded;
                discord.GuildMemberRemoved += feed.GuildMemberRemoved;
            }

            await discord.StartAsync();
            await Task.Delay(-1);
        }
    }
}