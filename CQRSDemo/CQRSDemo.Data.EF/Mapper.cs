using CQRSDemo.Data.EF.Entities;
using CQRSDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Data.EF
{
    public static class Mapper
    {
        public static Product ToEntity(this ProductModel model)
        {
            if (model == null) return null;
            return new Product()
            {
                ProductBrand = model.ProductBrand,
                ProductCategory = model.ProductCategory,
                ProductDescription = model.ProductDescription,
                ProductId = model.ProductId,
                ProductImage = model.ProductImage,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductType = model.ProductType
            };
        }
        public static ProductModel ToModel(this Product model)
        {
            if (model == null) return null;
            return new ProductModel()
            {
                ProductBrand = model.ProductBrand,
                ProductCategory = model.ProductCategory,
                ProductDescription = model.ProductDescription,
                ProductId = model.ProductId,
                ProductImage = model.ProductImage,
                ProductName = model.ProductName,
                ProductPrice = model.ProductPrice,
                ProductType = model.ProductType
            };
        }
    }
}
