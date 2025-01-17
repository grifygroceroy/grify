using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Persistence
{
    public class DbDetail
    {
        public DbDetail()
        {
            Server = Environment.GetEnvironmentVariable("sqlServer") ?? string.Empty;
            UserPassword = Environment.GetEnvironmentVariable("password") ?? string.Empty;
            DatabaseName = Environment.GetEnvironmentVariable("database") ?? string.Empty;
            UserName = Environment.GetEnvironmentVariable("sqlUser") ?? string.Empty;
            int.TryParse(Environment.GetEnvironmentVariable("maxPoolsize"), out int maxPoolsize);
            ConnectionPoolSize = maxPoolsize > 0 ? maxPoolsize : 100;
        }

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public int? ConnectionPoolSize { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"Data Source=(localdb)\\Local;Initial Catalog=Grify; Integrated Security=true";
                //return $"Server={Server};Initial Catalog={DatabaseName};User ID={UserName};Password={UserPassword};Max Pool Size={ConnectionPoolSize}";
            }
        }
    }
}
