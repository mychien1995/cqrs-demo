using CQRSDemo.Engine.Commands;
using CQRSDemo.Engine.Queries;
using CQRSDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace CQRSDemo.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Index(ProductSearchCriteria criteria)
        {
            var query = new SearchProductQuery(criteria);
            var result = query.Execute();
            return View(result);
        }

        // POST api/values
        [HttpPost]
        public ActionResult Insert(ProductModel model)
        {
            var command = new CreateProductCommand(model);
            command.Execute();
            return RedirectToAction("Index");
        }
    }
}
