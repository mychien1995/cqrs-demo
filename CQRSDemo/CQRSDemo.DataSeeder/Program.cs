using CQRSDemo.Data.EF.Entities;
using CQRSDemo.Data.EF.Repositories;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSDemo.DataSeeder
{
    public class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();
            //Put csv file here to run
            var csvFilePath = ConfigurationManager.AppSettings["CsvPath"] ??  @"D:\Download\womens-shoes-prices\7210_1.csv";
            var repository = new ProductRepository();
            using (var reader = new StreamReader(csvFilePath))
            {
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<dynamic>();
                    for (var j = 0; j < 10; j++)
                    {
                        int batch = 100;
                        int count = 0;
                        var batchList = new List<Product>();
                        foreach (var item in records)
                        {
                            count++;
                            var product = new Product()
                            {
                                ProductBrand = item.brand,
                                ProductName = item.name,
                                ProductCategory = item.categories,
                                ProductType = item.colors != null ? ((string)item.colors).Split(',').First() : "",
                                ProductPrice = Math.Floor((decimal)(rand.NextDouble() * 100)) / 100,
                                ProductImage = item.imageURLs != null ? ((string)item.imageURLs).Split(',').First() : ""
                            };
                            repository.Insert(product);
                            if (count == batch)
                            {
                                try
                                {
                                    count = 0;
                                    repository.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("DONE");
        }
    }
}
