using System.Collections.Generic;
using DatabaseWrapper.Core;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace Polaris.Classes
{
    public class GuildConfig
    {
        public static Dictionary<ulong, GuildConfig> Guilds = new();
        
        /// <summary>
        /// Returs 0 or 1 if a server is configured
        /// </summary>
        [JsonProperty("configured")]
        public int configured;
        
        /// <summary>
        /// Id of the message wich contains the rules
        /// </summary>
        [JsonProperty("rulesid")]
        public ulong rulesmessage;

        /// <summary>
        /// The channel where the bot announce arrivals/departures
        /// </summary>
        [JsonProperty("welcomechannelid")]
        public ulong welcomechannelid;

        #region Constructor

        public GuildConfig(int configured, ulong rulesmessage = default, ulong welcomechannelid = default)
        {
            this.configured = configured;
            this.rulesmessage = rulesmessage;
            this.welcomechannelid = welcomechannelid;
        }

        #endregion
    }
}