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
        public ConnectionMultiplexer RedisConnection;
        public static string ConnectionString
        {
            get
            {
                return Endpoint + ",allowAdmin=true";
            }
        }

        public static string Endpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["RedisConnection"] ?? "localhost:6379";
            }
        }
        public IDatabase Database
        {
            get
            {
                return RedisConnection.GetDatabase();
            }
        }
        public RedisContext()
        {
            RedisConnection = ConnectionMultiplexer.Connect(ConnectionString);
        }

        public void Dispose()
        {
            RedisConnection.Close();
            RedisConnection.Dispose();
        }

        public RedisKey[] GetKeys(string pattern)
        {
            return this.RedisConnection.GetServer(Endpoint).Keys(pattern: pattern).ToArray();
        }
    }
}
