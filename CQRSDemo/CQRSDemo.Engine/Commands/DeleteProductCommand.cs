using CQRSDemo.Data.EF.Repositories;
using CQRSDemo.Engine.Elastic;
using CQRSDemo.Engine.Redis;
using CQRSDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Commands
{
    public class DeleteProductCommand : ICommand<bool>
    {
        private int ID;
        private readonly IProductRepository _productRepository;
        public DeleteProductCommand(int idToDelete)
        {
            ID = idToDelete;
            _productRepository = new ProductRepository();
        }
        public bool Execute()
        {
            DeleteFromDatabase();
            DeleteFromElastic();
            DeleteFromRedis();
            return true;
        }

        private void DeleteFromDatabase()
        {
            _productRepository.Delete(ID);
            _productRepository.SaveChanges();
        }

        private void DeleteFromElastic()
        {
            new ContentIndexer().Delete(ID);
        }

        private void DeleteFromRedis()
        {
            using (var context = new RedisContext())
            {
                var cacheKey = new ProductModel()
                {
                    ProductId = ID
                }.CacheKey;
                context.Database.KeyDelete(cacheKey);
                var searchKeys = context.GetKeys("ProductSearch-*");
                foreach (var key in searchKeys)
                {
                    context.Database.KeyDelete(key);
                }
            }
        }
    }
}
