using System.Collections.Generic;
using DatabaseWrapper.Core;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace Polaris.Classes
{
    public class Config
    {
        private Database _database;
        public Dictionary<ulong, GuildConfig> Guilds = new();
        public string[] prefix;
        public void AddGuildConfig(DiscordGuild guild)
        {
            
            var values = new Dictionary<string, object>();
            values.Add("guildid", guild.Id);
            values.Add("config", JsonConvert.SerializeObject(new GuildConfig(0)));
            _database.Client.Insert("guilds", values);
            
            Guilds.Add(guild.Id, new GuildConfig(0));
        }
        
        public void UpdateGuildConfig(DiscordGuild discordGuild, GuildConfig newGuildConfig)
        {
            Guilds[discordGuild.Id] = newGuildConfig;

            var values = new Dictionary<string, object>();
            values.Add("config", JsonConvert.SerializeObject(newGuildConfig));
            var expression = new Expression("guildid", Operators.Equals, discordGuild.Id);
            _database.Client.Update("guilds", values, expression);
        }

        public void DeleteGuildConfig(DiscordGuild discordGuild)
        {
            Guilds.Remove(discordGuild.Id);
            
            var expression = new Expression("guildid", Operators.Equals, discordGuild.Id);
            _database.Client.Delete("guilds", expression);
        }

        public Config(Database database, string[] prefix)
        {
            _database = database;
            this.prefix = prefix;
        }
    }
}