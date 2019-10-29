using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Models
{
    public class ProductSearchCriteria : IRedisCachable
    {
        public ProductSearchCriteria()
        {
            PageIndex = 0;
            PageSize = 5000;
        }
        public string Text { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string CacheKey
        {
            get
            {
                return $"Product-[{Text}]-[{Type}]-[{Category}]-[{Brand}]-[{PageIndex}]-[{PageSize}]";
            }
        }

        public static ProductSearchCriteria Default
        {
            get
            {
                return new ProductSearchCriteria()
                {
                    PageIndex = 0,
                    PageSize = 10
                };
            }
        }
    }
}
