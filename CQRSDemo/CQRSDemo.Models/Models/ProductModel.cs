using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Models
{
    public class ProductModel : IRedisCachable
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public decimal? ProductPrice { get; set; }
        public string ProductImage { get; set; }

        public string CacheKey
        {
            get
            {
                return "ProductModel-" + ProductId;
            }
        }
    }
}
