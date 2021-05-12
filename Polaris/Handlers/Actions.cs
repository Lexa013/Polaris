using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseWrapper.Core;
using DatabaseWrapper.Mysql;
using DSharpPlus;
using DSharpPlus.EventArgs;
using GameCore;
using Newtonsoft.Json;
using Polaris.Classes;
using Polaris.Managers;
using Console = System.Console;

namespace Polaris.Handlers
{
    public class Actions
    {
        
        private GuildManager guildManager;

        public Task GuildJoined(DiscordClient sender, GuildCreateEventArgs e)
        {
            guildManager.AddGuildConfig(e.Guild);
            
            return null;
        }

        public Task GuildDeleted(DiscordClient sender, GuildDeleteEventArgs e)
        {
            guildManager.DeleteGuildConfig(e.Guild);
            
            return null;
        }

        public async Task GuildDownloadCompleted(DiscordClient sender, GuildDownloadCompletedEventArgs e)
        {
            var result = guildManager.getGuildsConfigs();
            
            Console.WriteLine($"Config rows count: {result.Rows.Count}");

            for (int i = 0; i < result.Rows.Count; i++)
            {
                GuildConfig.Guilds.Add(Convert.ToUInt64(result.Rows[i].ItemArray.GetValue(0)),
                    JsonConvert.DeserializeObject<GuildConfig>(result.Rows[i].ItemArray.GetValue(1).ToString()));
            }
        }
        
        public Actions(GuildManager guildManager)
        {
            this.guildManager = guildManager;
        }
    }
}