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
            var response = client.Indices.Create(IndexName, index => index
            .Map<ProductIndexDocument>(
                x => x.AutoMap()
                .Properties(p => p.Text(
                    t => t.Name(n => n.ProductName)
                    .Analyzer("wildcard_analyzer")
                    .SearchAnalyzer("wildcard_analyzer")
                )
            ))
            .Settings(st => st
                .Analysis(an => an
                    .Analyzers(anz => anz
                        .Custom("ngram_analyzer", cc => cc
                            .Tokenizer("standard")
                            .Filters(new List<string> { "lowercase", "myNGram" }))
                        .Custom("ngram_search_analyzer", cc => cc
                            .Tokenizer("standard")
                            .Filters(new List<string> { "standard", "lowercase", "myNGram" }))
                        .Custom("wildcard_analyzer", cc => cc
                            .Tokenizer("keyword")
                            .Filters(new List<string> { "lowercase" }))
                    )
                    .TokenFilters(
                         t => t.NGram("myNGram", x => x.MinGram(2).MaxGram(20))
                    )
                )
                .Setting(UpdatableIndexSettings.MaxNGramDiff, 20)
           )
         );
        }
    }
}
