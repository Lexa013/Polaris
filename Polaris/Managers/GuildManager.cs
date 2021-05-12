using System.Collections.Generic;
using System.Data;
using DatabaseWrapper.Core;
using DatabaseWrapper.Mysql;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Polaris.Classes;

namespace Polaris.Managers
{
    public class GuildManager
    {
        private Config Config;

        public void AddGuildConfig(DiscordGuild guild)
        {
            var values = new Dictionary<string, object>();
            values.Add("guildid", guild.Id);
            values.Add("config", JsonConvert.SerializeObject(new GuildConfig(0)));
            Config.Database.Insert("guilds", values);
            
            GuildConfig.Guilds.Add(guild.Id, new GuildConfig(0));
        }
        
        public void UpdateGuildConfig(DiscordGuild discordGuild, GuildConfig newGuildConfig)
        {
            GuildConfig.Guilds[discordGuild.Id] = newGuildConfig;

            var values = new Dictionary<string, object>();
            values.Add("config", JsonConvert.SerializeObject(newGuildConfig));
            var expression = new Expression("guildid", Operators.Equals, discordGuild.Id);
            Config.Database.Update("guilds", values, expression);
        }

        public void DeleteGuildConfig(DiscordGuild discordGuild)
        {
            GuildConfig.Guilds.Remove(discordGuild.Id);
            
            var expression = new Expression("guildid", Operators.Equals, discordGuild.Id);
            Config.Database.Delete("guilds", expression);
        }

        public DataTable getGuildsConfigs()
        {
            var fields = new List<string> {"guildid", "config"};
            var result = Config.Database.Select("guilds", null, null, fields, null);

            return result;
        }

        public GuildManager(Config Config)
        {
            this.Config = Config;
        }
    }
}