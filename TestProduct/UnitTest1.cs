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
            var actionName = "GetById";
            var idValues = 4;
            var controller = new ProductController(new ProductService());
            var actionResult = controller.PostProduct(product);

            Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal(actionName, nameof(controller.GetById));
            var okObject = actionResult as OkObjectResult;
            Assert.NotNull(okObject);
            var resultProduct = okObject.Value as Product;
            Assert.NotNull(resultProduct);
            Assert.Equal(idValues, (resultProduct.Id));

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
            Assert.IsType<BadRequestObjectResult>(actionResult);
            var okObject = actionResult as OkObjectResult;
            Assert.NotNull(okObject);
            var resultProduct = okObject.Value as Product;
            Assert.NotNull(resultProduct);


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
            })).ToArray();
            Task results = Task.WhenAll(tasks);
        }
        [Fact]
        public void Test4()
        {
        }
    }
}
