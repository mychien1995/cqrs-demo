using CQRSDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSDemo.Presentation.Models
{
    public class SearchResultViewModel<T>
    {
        public SearchResult<T> Data { get; set; }
        public long QueryTime { get; set; }

        public long SqlQueryTime { get; set; }
        public long RedisQueryTime { get; set; }
    }
}