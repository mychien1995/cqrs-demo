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
    public class GetProductByIdQuery : IQuery<ProductModel>
    {
        private int ProductId;
        public GetProductByIdQuery(int id)
        {
            ProductId = id;
        }
        public ProductModel Execute()
        {
            using (var context = new RedisContext())
            {
                var product = new ProductModel();
                product.ProductId = ProductId;
                var cachedValue = context.Database.StringGet(product.CacheKey);
                if (cachedValue.IsNullOrEmpty)
                {
                    var searcher = new ContentSearcher();
                    var productModel = searcher.GetProductById(this.ProductId);
                    context.Database.StringSet(productModel.CacheKey, JsonConvert.SerializeObject(productModel));
                    return productModel;
                }
                return JsonConvert.DeserializeObject<ProductModel>(cachedValue);
            }
        }
    }
}
