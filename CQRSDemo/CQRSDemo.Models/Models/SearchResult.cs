using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Models
{
    public class SearchResult<T>
    {
        public SearchResult()
        {
            Total = 0;
            Result = new List<T>();
        }
        public int Total { get; set; }
        public List<T> Result { get; set; }
    }
}
