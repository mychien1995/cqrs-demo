using CQRSDemo.Data.EF;
using CQRSDemo.Data.EF.Repositories;
using CQRSDemo.Engine.Commands;
using CQRSDemo.Engine.Elastic;
using CQRSDemo.Engine.Queries;
using CQRSDemo.Engine.Redis;
using CQRSDemo.Models;
using CQRSDemo.Presentation.Models;
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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var query = new SearchProductQuery(criteria);
            var result = query.Execute();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var viewModel = new SearchResultViewModel<ProductModel>();
            viewModel.Data = result;
            viewModel.QueryTime = elapsedMs;
            ViewBag.Heading = "Query with Redis and Elastic Search";
            ViewBag.IsSql = false;
            return View(viewModel);
        }

        public ActionResult SqlIndex(ProductSearchCriteria criteria)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var query = new SqlSearchProductQuery(criteria);
            var result = query.Execute();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var viewModel = new SearchResultViewModel<ProductModel>();
            viewModel.Data = result;
            viewModel.QueryTime = elapsedMs;
            ViewBag.Heading = "Query with SQL";
            ViewBag.IsSql = true;
            return View("~/Views/Products/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Search(ProductSearchCriteria criteria)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var query = new SearchProductQuery(criteria);
            var result = query.Execute();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var viewModel = new SearchResultViewModel<ProductModel>();
            viewModel.Data = result;
            viewModel.QueryTime = elapsedMs;
            return PartialView("~/Views/Products/_Search.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult SqlSearch(ProductSearchCriteria criteria)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var query = new SqlSearchProductQuery(criteria);
            var result = query.Execute();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var viewModel = new SearchResultViewModel<ProductModel>();
            viewModel.Data = result;
            viewModel.QueryTime = elapsedMs;
            return PartialView("~/Views/Products/_Search.cshtml", viewModel);
        }
        [HttpGet]
        public ActionResult Insert()
        {
            var model = new ProductModel();
            return View(model);
        }
        // POST api/values
        [HttpPost]
        public ActionResult Insert(ProductModel model)
        {
            var command = new CreateProductCommand(model);
            command.Execute();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var query = new GetProductByIdQuery(id);
            var product = query.Execute();
            return View(product);
        }

        [HttpPost]
        public ActionResult Update(ProductModel model)
        {
            var command = new CreateProductCommand(model);
            command.Execute();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Reindex()
        {
            var contentIndexer = new ContentIndexer();
            var products = new ProductRepository().GetAll().Select(x => x.ToModel()).ToList();
            var count = 0;
            var batch = 100;
            var productList = new List<ProductModel>();
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                count++;
                productList.Add(product);
                if (count == batch || i == products.Count - 1)
                {
                    count = 0;
                    contentIndexer.IndexMany(productList);
                    productList = new List<ProductModel>();
                }
            }
            using (var context = new RedisContext())
            {
                context.Database.KeyDelete("*");
            }
            return new EmptyResult();
        }
    }
}
