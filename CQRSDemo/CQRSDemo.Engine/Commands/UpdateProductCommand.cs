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
    public class UpdateProductCommand : ICommand<ProductModel>
    {
        private ProductModel Product;
        private readonly IProductRepository _productRepository;
        public UpdateProductCommand(ProductModel productToCreate)
        {
            Product = productToCreate;
            _productRepository = new ProductRepository();
        }
        public ProductModel Execute()
        {
            var product = this.Product.ToEntity();
            var newProduct = UpdateToDatabase(product);
            UpdateToElasticSearch(newProduct);
            UpdateToRedis(newProduct);
            return newProduct;
        }

        private ProductModel UpdateToDatabase(Product product)
        {
            _productRepository.Update(product);
            _productRepository.SaveChanges();
            return product.ToModel();
        }
        private void UpdateToRedis(ProductModel product)
        {
            using (var context = new RedisContext())
            {
                var value = JsonConvert.SerializeObject(product);
                context.Database.StringSet(product.CacheKey, value);
            }
        }
        private void UpdateToElasticSearch(ProductModel product)
        {
            new ContentIndexer().Index(product);
        }
    }
}
