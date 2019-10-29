using CQRSDemo.Engine.Models;
using CQRSDemo.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Elastic
{
    public class ElasticSearchContext
    {
        private static Uri Node = new Uri(ConfigurationManager.AppSettings["ELConnection"] ?? "http://localhost:9200/");

        private static bool _initialized = false;
        public static string IndexName = "cqrs";

        public static ElasticClient Client
        {
            get
            {
                if (!_initialized)
                {
                    Initialize();
                    _initialized = true;
                }
                var settings = new ConnectionSettings(Node);
                settings.DefaultIndex(IndexName);
                var client = new ElasticClient(settings);
                return client;
            }
        }

        private static void Initialize()
        {
            var client = new ElasticClient(Node);
            client.Indices.Create(IndexName, index => index.Map<ProductIndexDocument>(x => x.AutoMap()));
        }
    }
}
