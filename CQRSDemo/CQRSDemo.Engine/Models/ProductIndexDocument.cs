using CQRSDemo.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Models
{

    [ElasticsearchType(IdProperty = nameof(ProductId))]
    public class ProductIndexDocument
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductBrand { get; set; }
        public string[] ProductCategories { get; set; }
        public string ProductType { get; set; }
        public decimal? ProductPrice { get; set; }
    }

    public static class ProductIndexDocumentExtensions
    {
        public static ProductIndexDocument ToDocument(this ProductModel model)
        {
            if (model == null) return null;
            return new ProductIndexDocument()
            {
                ProductBrand = model.ProductBrand,
                ProductCategories = model.ProductCategory != null ? model.ProductCategory.Split(',') : null,
                ProductDescription = model.ProductDescription,
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductType = model.ProductType
            };
        }


        public static ProductModel ToModel(this ProductIndexDocument model)
        {
            if (model == null) return null;
            return new ProductModel()
            {
                ProductBrand = model.ProductBrand,
                ProductCategory = model.ProductCategories != null ? string.Join(",", model.ProductCategories) : null,
                ProductDescription = model.ProductDescription,
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductType = model.ProductType
            };
        }
    }
}
