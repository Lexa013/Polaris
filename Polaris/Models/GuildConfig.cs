using System.Collections.Generic;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace Polaris.Models
{
    public class GuildConfig
    {
        public static Dictionary<ulong, GuildConfig> Guilds = new();

        /// <summary>
        /// Id of the message wich contains the rules
        /// </summary>
        [JsonProperty("rulesid")] public DiscordMessage rulesmessage;

        /// <summary>The role given to the user when he's checking the reaction</summary>
        [JsonProperty("verifiedrole")] public DiscordRole verifiedrole;
        
        /// <summary>
        /// The channel where the bot announce arrivals/departures
        /// </summary>
        [JsonProperty("welcomechannel")] public DiscordChannel welcomechannelid;
        
        /// <summary>
        /// The message wich will be displayed in the welcomechannelid when a member join the guild
        /// </summary>
        [JsonProperty("welcomemessage")] public string welcomemessage;

        /// <summary>
        ///  The message wich will be displayed in the welcomechannelid when a member join the guild
        /// </summary>
        [JsonProperty("leavemessage")] public string leavemessage;
    }
}