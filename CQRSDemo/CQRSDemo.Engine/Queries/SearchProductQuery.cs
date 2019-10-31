using CQRSDemo.Data.EF;
using CQRSDemo.Data.EF.Repositories;
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
                    Task.Run(() =>
                    {
                        using (var tmpContext = new RedisContext())
                        {
                            var serializedValue = JsonConvert.SerializeObject(searchResult);
                            tmpContext.Database.StringSet(Query.CacheKey, serializedValue);
                        }
                    });
                    return searchResult;
                }
                var result = JsonConvert.DeserializeObject<SearchResult<ProductModel>>(cachedObject);
                return result;
            }
        }

        //public SearchResult<ProductModel> Execute()
        //{
        //    var productRepository = new ProductRepository();
        //    using (var context = new RedisContext())
        //    {
        //        var cachedObject = context.Database.StringGet(Query.CacheKey);
        //        if (cachedObject.IsNullOrEmpty)
        //        {
        //            var searcher = new ContentSearcher();
        //            SearchResult<int> searchResult = searcher.SearchIdOnly(Query);
        //            var redisResult = new SearchResult<ProductModel>();
        //            foreach (var item in searchResult.Result)
        //            {
        //                var cachedItem = context.Database.StringGet("ProductModel-" + item);
        //                if (cachedItem.IsNullOrEmpty)
        //                {
        //                    var actualProduct = productRepository.GetById(item).ToModel();
        //                    redisResult.Result.Add(actualProduct);
        //                    var serializedValue = JsonConvert.SerializeObject(actualProduct);
        //                    context.Database.StringSet(actualProduct.CacheKey, serializedValue);
        //                }
        //                else redisResult.Result.Add(JsonConvert.DeserializeObject<ProductModel>(cachedItem));
        //            }
        //            Task.Run(() =>
        //            {
        //                using (var tmpContext = new RedisContext())
        //                {
        //                    var serializedValue = JsonConvert.SerializeObject(redisResult);
        //                    tmpContext.Database.StringSet(Query.CacheKey, serializedValue);
        //                }
        //            });
        //            return redisResult;
        //        }
        //        var result = JsonConvert.DeserializeObject<SearchResult<ProductModel>>(cachedObject);
        //        return result;
        //    }
        //}
    }
}
