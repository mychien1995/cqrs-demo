﻿using CQRSDemo.Engine.Models;
using CQRSDemo.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.Engine.Elastic
{
    public class ContentIndexer
    {
        public void Index(ProductModel model)
        {
            var client = ElasticSearchContext.Client;
            var document = model.ToDocument();
            var response = client.IndexDocument(document);
        }

        public void IndexMany(IEnumerable<ProductModel> models)
        {
            var client = ElasticSearchContext.Client;
            var documents = models.Select(x => x.ToDocument());
            var test = documents.First();
            var response = client.IndexMany(documents);
            if (!response.ApiCall.Success)
            {

            }
        }
    }
}
