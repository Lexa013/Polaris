using DatabaseWrapper.Mysql;
using Newtonsoft.Json;

namespace Polaris.Classes
{
    public class Database
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
        private Database _database { get; set; }
        
        [JsonProperty("prefix")]
        public string Prefix { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonIgnore]
        public DatabaseClient Database => new DatabaseClient(_database.Host, _database.Port, _database.Username,
            _database.Password, _database.DatabaseName);
    }
}