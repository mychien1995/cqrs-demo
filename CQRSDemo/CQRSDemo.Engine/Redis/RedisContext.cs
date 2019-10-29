using StackExchange.Redis;
using System;
using System.Collections.Generic;
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
            RedisConnection = ConnectionMultiplexer.Connect("localhost");
        }

        public void Dispose()
        {
            RedisConnection.Close();
            RedisConnection.Dispose();
        }
    }
}
