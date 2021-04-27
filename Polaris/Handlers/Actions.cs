using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseWrapper.Core;
using DSharpPlus;
using DSharpPlus.EventArgs;
using GameCore;
using Newtonsoft.Json;
using Polaris.Classes;
using Console = System.Console;

namespace Polaris.Handlers
{
    public class Actions
    {

        private Database _Database;
        private Config _Config;

        public Task GuildJoined(DiscordClient sender, GuildCreateEventArgs e)
        {
            _Config.AddGuildConfig(e.Guild);
            
            return null;
        }

        public Task GuildDeleted(DiscordClient sender, GuildDeleteEventArgs e)
        {
            _Config.DeleteGuildConfig(e.Guild);
            
            return null;
        }

        public async Task GuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
        {
            var fields = new List<string> {"guildid", "config"};
            var result = _Database.Client.Select("guilds", null, null, fields, null);
            
            Console.WriteLine($"Config rows count: {result.Rows.Count}");

            for (int i = 0; i < result.Rows.Count; i++)
            {
                _Config.Guilds.Add(Convert.ToUInt64(result.Rows[i].ItemArray.GetValue(0)),
                    JsonConvert.DeserializeObject<GuildConfig>(result.Rows[i].ItemArray.GetValue(1).ToString()));
            }
        }
        
        public Actions(Database database, Config config)
        {
            _Database = database;
            _Config = config;
        }
    }
}