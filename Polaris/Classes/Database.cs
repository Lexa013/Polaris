using DatabaseWrapper;
using DatabaseWrapper.Core;

namespace Polaris.Classes
{
    public class Database
    {
        public DbTypes type; 
        public string hostname;
        public int port;
        public string user;
        private string password;
        public string database;
        public DatabaseClient Client;

        public Database(DbTypes type, string hostname, int port, string user, string password, string database)
        {
            this.type = type;
            this.hostname = hostname;
            this.port = port;
            this.user = user;
            this.password = password;
            this.database = database;

            Client = new DatabaseClient(
                this.type,
                this.hostname,
                this.port,
                this.user,
                this.password,
                this.database
            );
        }
    }
}