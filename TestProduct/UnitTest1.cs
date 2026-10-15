using Microsoft.AspNetCore.Components;
using ProductAPI.Controllers;
using ProductAPI.Service;
using ProductAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestProduct
{
    public class UnitTest1
    {
        [Fact]
        public void TestCreatedProduct()
        {
            var product = new Product()
            {
                Name = "Test",
                Price = 100,
                Stock = 2
            };


            var controller = new ProductController(new ProductService());
            var actionResult = controller.PostProduct(product);
      
            Assert.IsType<CreatedAtActionResult>(actionResult);
            
            var okObject = actionResult as CreatedAtActionResult;
            Assert.NotNull(okObject);
            var actionName = okObject.ActionName;
            Assert.Equal(actionName, nameof(controller.GetById));
            var resultProduct = okObject.Value as Product;
            Assert.NotNull(resultProduct);
            Assert.Equal(4, okObject.RouteValues["id"]);

        }
        [Fact]
        public void TestCreatedProductNegativePrice()
        {
            var product = new Product()
            {
                Name = "Test",
                Price = -100,
                Stock = 2
            };
            var controller = new ProductController(new ProductService());
            var actionResult = controller.PostProduct(product);
            var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult);
            var errors = Assert.IsType<List<string>>(badRequest.Value);
            Assert.Contains("Цена должа быть больше нуля", errors);

        }
        [Fact]
        public async Task AddProductsConcurrently()
        {

            var service = new ProductService();

            var tasks = Enumerable.Range(0, 100).Select(i => Task.Run(() =>
            {
                Product product = new Product
                {
                    Name = $"Product{i}",
                    Price = new Random().Next(1, 100),
                    Stock = new Random().Next(1, 100)
                };
                return service.AddProduct(product);
            })).ToArray();
            var results = await Task.WhenAll(tasks);
            Assert.All(results, result => Assert.True(result.Success));
            var products = service.GetProducts().ToList();
            Assert.Equal(103, products.Count);
            Assert.Equal(products.Count, products.Select(p=>p.Id).Distinct().Count());  
        }
        [Fact]
        public void Test4()
        {
        }
    }
}
