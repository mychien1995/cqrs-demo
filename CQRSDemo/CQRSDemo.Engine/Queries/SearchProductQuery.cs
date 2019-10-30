using CQRSDemo.Engine.Elastic;
using CQRSDemo.Engine.Redis;
using CQRSDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Queries
{
    public class SearchProductQuery : IQuery<SearchResult<ProductModel>>
    {
        private ProductSearchCriteria Query;
        public SearchProductQuery(ProductSearchCriteria query)
        {
            Query = query;
            if (Query == null) Query = ProductSearchCriteria.Default;
        }
        public SearchResult<ProductModel> Execute()
        {
            using (var context = new RedisContext())
            {
                var cachedObject = context.Database.StringGet(Query.CacheKey);
                if (cachedObject.IsNullOrEmpty)
                {
                    var searcher = new ContentSearcher();
                    //Search with elastic search
                    SearchResult<ProductModel> searchResult = searcher.Search(Query);
                    var serializedValue = JsonConvert.SerializeObject(searchResult);
                    context.Database.StringSet(Query.CacheKey, serializedValue);
                    return searchResult;
                }
                var result = JsonConvert.DeserializeObject<SearchResult<ProductModel>>(cachedObject);
                return result;
            }
        }
    }
}
