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
    public class CompareController : Controller
    {
        public ActionResult Index(ProductSearchCriteria criteria)
        {
            var sqlQuery = new SqlSearchProductQuery(criteria);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = sqlQuery.Execute();
            watch.Stop();
            var sqlElapsedMs = watch.ElapsedMilliseconds;

            var redisQuery = new SearchProductQuery(criteria);
            watch = System.Diagnostics.Stopwatch.StartNew();
            result = redisQuery.Execute();
            watch.Stop();
            var redisElapsedMs = watch.ElapsedMilliseconds;

            var viewModel = new SearchResultViewModel<ProductModel>();
            viewModel.Data = result;
            viewModel.SqlQueryTime = sqlElapsedMs;
            viewModel.RedisQueryTime = redisElapsedMs;
            ViewBag.Heading = "Compare View";
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Search(ProductSearchCriteria criteria)
        {
            var sqlQuery = new SqlSearchProductQuery(criteria);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = sqlQuery.Execute();
            watch.Stop();
            var sqlElapsedMs = watch.ElapsedMilliseconds;

            var redisQuery = new SearchProductQuery(criteria);
            watch = System.Diagnostics.Stopwatch.StartNew();
            result = redisQuery.Execute();
            watch.Stop();
            var redisElapsedMs = watch.ElapsedMilliseconds;

            var viewModel = new SearchResultViewModel<ProductModel>();
            viewModel.Data = result;
            viewModel.SqlQueryTime = sqlElapsedMs;
            viewModel.RedisQueryTime = redisElapsedMs;
            return PartialView("~/Views/Compare/_Search.cshtml", viewModel);
        }
    }
}
