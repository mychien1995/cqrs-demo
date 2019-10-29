using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Redis
{
    public class RedisContext : IDisposable
    {
        private readonly ConnectionMultiplexer RedisConnection;
        public IDatabase Database
        {
            get
            {
                return RedisConnection.GetDatabase();
            }
        }
        public RedisContext()
        {
            var connString = ConfigurationManager.AppSettings["RedisConnection"] ?? "localhost:6379";
            RedisConnection = ConnectionMultiplexer.Connect(connString);
        }

        public void Dispose()
        {
            RedisConnection.Close();
            RedisConnection.Dispose();
        }
    }
}
