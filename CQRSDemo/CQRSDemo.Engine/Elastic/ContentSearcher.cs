using CQRSDemo.Engine.Models;
using CQRSDemo.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Elastic
{
    public class ContentSearcher
    {
        public ProductIndexDocument GetProductDocumentById(int id)
        {
            var client = ElasticSearchContext.Client;
            var response = client.Search<ProductIndexDocument>(x => x.Query(c => c.Bool(b => b.Must(a => a.Term(n => n.ProductId, id)))));
            var projects = response.Documents;
            return projects.FirstOrDefault();
        }
        public ProductModel GetProductById(int id)
        {
            var document = GetProductDocumentById(id);
            return document.ToModel();
        }

        public SearchResult<int> SearchIdOnly(ProductSearchCriteria criteria)
        {
            var client = ElasticSearchContext.Client;
            var queries = new List<Func<QueryContainerDescriptor<ProductIndexDocument>, QueryContainer>>();
            if (!string.IsNullOrEmpty(criteria.Text))
            {
                queries.Add(x => x.Match(m => m.Field(f => f.ProductName).Query(criteria.Text)));
            }
            if (!string.IsNullOrEmpty(criteria.Brand))
            {
                queries.Add(x => x.Match(m => m.Field(f => f.ProductBrand).Query(criteria.Brand)));
            }
            if (!string.IsNullOrEmpty(criteria.Category))
            {
                queries.Add(x => x.Match(m => m.Field(f => f.ProductCategories).Query(criteria.Category)));
            }
            if (!string.IsNullOrEmpty(criteria.Type))
            {
                queries.Add(x => x.Match(m => m.Field(f => f.ProductType).Query(criteria.Type)));
            }

            var response = client.Search<ProductIndexDocument>(
                x => x
                .Query(
                    c => c.Bool(
                        b => b.Must(
                            queries
                        )
                        )
                    ).Sort(s => s.Descending(c => c.ProductId)).Skip(criteria.PageIndex * criteria.PageSize).Take(criteria.PageSize)
                );
            var projects = response.Documents;
            var data = projects.Select(x => x.ProductId).ToList();
            var result = new SearchResult<int>();
            result.Result = data;
            return result;
        }

        public SearchResult<ProductModel> Search(ProductSearchCriteria criteria)
        {
            var client = ElasticSearchContext.Client;
            var queries = new List<Func<QueryContainerDescriptor<ProductIndexDocument>, QueryContainer>>();
            if (!string.IsNullOrEmpty(criteria.Text))
            {
                criteria.Text = "*" + criteria.Text.ToLower() + "*";
            }
            var response = string.IsNullOrEmpty(criteria.Text) ? client.Search<ProductIndexDocument>(
                x => x.Sort(s => s.Descending(c => c.ProductId)).Skip(criteria.PageIndex * criteria.PageSize).Take(criteria.PageSize)
                ) : client.Search<ProductIndexDocument>(
                x => x.Query(
                    c => c.Wildcard(w => w.Field(f => f.ProductName).Value(criteria.Text))
                    ).Skip(criteria.PageIndex * criteria.PageSize).Take(criteria.PageSize)
                );
            var projects = response.Documents;
            var data = projects.Select(x => x.ToModel()).ToList();
            var result = new SearchResult<ProductModel>();
            result.Result = data;
            return result;
        }
    }
}
