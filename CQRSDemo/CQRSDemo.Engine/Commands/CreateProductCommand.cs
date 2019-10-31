using CQRSDemo.Data.EF;
using CQRSDemo.Data.EF.Entities;
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

namespace CQRSDemo.Engine.Commands
{
    public class CreateProductCommand : ICommand<ProductModel>
    {
        private ProductModel Product;
        private readonly IProductRepository _productRepository;
        public CreateProductCommand(ProductModel productToCreate)
        {
            Product = productToCreate;
            _productRepository = new ProductRepository();
        }
        public ProductModel Execute()
        {
            var product = this.Product.ToEntity();
            var newProduct = InsertToDatabase(product);
            InsertToElasticSearch(newProduct);
            InsertToRedis(newProduct);
            return newProduct;
        }

        private ProductModel InsertToDatabase(Product product)
        {
            _productRepository.Insert(product);
            _productRepository.SaveChanges();
            return product.ToModel();
        }
        private void InsertToRedis(ProductModel product)
        {
            using (var context = new RedisContext())
            {
                var value = JsonConvert.SerializeObject(product);
                context.Database.StringSet(product.CacheKey, value);
                var searchKeys = context.GetKeys("ProductSearch-*");
                foreach (var key in searchKeys)
                {
                    context.Database.KeyDelete(key);
                }
            }
        }
        private void InsertToElasticSearch(ProductModel product)
        {
            new ContentIndexer().Index(product);
        }
    }
}
