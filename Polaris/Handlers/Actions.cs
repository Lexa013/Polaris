using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using Polaris.Models;
using Polaris.Managers;
using Console = System.Console;

namespace Polaris.Handlers
{
    public class Actions
    {
        
        private GuildManager _guildManager;

        public Task GuildJoined(DiscordClient sender, GuildCreateEventArgs e)
        {
            _guildManager.AddGuildConfig(e.Guild);
            
            return null;
        }

        public Task GuildDeleted(DiscordClient sender, GuildDeleteEventArgs e)
        {
            _guildManager.DeleteGuildConfig(e.Guild.Id);
            
            return null;
        }

        public async Task GuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
        {
            // Get guilds configs stored in the database
            var result = _guildManager.GetGuildsConfigs();

            // Add them to a static dictionary <ulong guildid, GuildConfig guildconfig>
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var guildId = Convert.ToUInt64(result.Rows[i].ItemArray.GetValue(0));
                var guildConfig =
                    JsonConvert.DeserializeObject<GuildConfig>(result.Rows[i].ItemArray.GetValue(1).ToString());
                
                GuildConfig.Guilds.Add(guildId, guildConfig);

                // Delete guild config if the bot isn't on the server in case if the bot is off
                if (!sender.Guilds.Keys.Contains(guildId))
                    _guildManager.DeleteGuildConfig(guildId);
            }
            
            // Add guild config if the guild config isn't in the database in case if the bot is off
            foreach (var guild in sender.Guilds)
            {
                if (!GuildConfig.Guilds.ContainsKey(guild.Key))
                    _guildManager.AddGuildConfig(guild.Value);
            }
            
            Console.WriteLine($"Config rows count: {GuildConfig.Guilds.Count}");
        }
        
        public Actions(GuildManager guildManager)
        {
            _guildManager = guildManager;
        }
    }
}