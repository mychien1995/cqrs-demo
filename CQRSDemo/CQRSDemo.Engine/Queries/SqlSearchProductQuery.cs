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
    public class SqlSearchProductQuery
    {
        private ProductSearchCriteria Query;
        public SqlSearchProductQuery(ProductSearchCriteria query)
        {
            Query = query;
            if (Query == null) Query = ProductSearchCriteria.Default;
        }
        public SearchResult<ProductModel> Execute()
        {
            var repo = new ProductRepository();
            var query = repo.GetQueryable();
            if (!string.IsNullOrEmpty(Query.Text))
            {
                query = query.Where(x => x.ProductName.ToLower().Contains(Query.Text.ToLower()));
            }
            if (!string.IsNullOrEmpty(Query.Brand))
            {
                query = query.Where(x => x.ProductBrand.ToLower().Contains(Query.Brand.ToLower()));
            }
            if (!string.IsNullOrEmpty(Query.Category))
            {
                query = query.Where(x => x.ProductCategory.ToLower().Contains(Query.Category.ToLower()));
            }
            if (!string.IsNullOrEmpty(Query.Type))
            {
                query = query.Where(x => x.ProductType.ToLower().Contains(Query.Type.ToLower()));
            }
            var result = query.OrderBy(x => x.ProductId).Skip(Query.PageSize * Query.PageIndex).Take(Query.PageSize).ToList().Select(x => x.ToModel()).ToList();
            var searchResult = new SearchResult<ProductModel>();
            searchResult.Result = result;
            return searchResult;
        }
    }
}
