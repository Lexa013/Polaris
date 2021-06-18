using DatabaseWrapper.Mysql;
using Newtonsoft.Json;

namespace Polaris.Models
{
    public class DatabaseConfig
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("database")]
        public string DatabaseName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class Config
    {
        [JsonProperty("database")]
        private DatabaseConfig _Database { get; set; }
        
        [JsonProperty("prefix")]
        public string Prefix { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("botowner")]
        public string botowner { get; set; }

        [JsonIgnore]
        public DatabaseClient Database { get; set; }

        public void Init()
        {
            Database = new DatabaseClient(_Database.Host, _Database.Port, _Database.Username,
                _Database.Password, _Database.DatabaseName);
        }
    }
}