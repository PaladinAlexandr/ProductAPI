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
        public void Test1()
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

        }
        [Fact]
        public void Test2()
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
        }
        [Fact]
        public void Test3()
        {
            var product = new Product()
            {
                Name = "Test",
                Price = 100,
                Stock = 2
            };
            var controller = new ProductController(new ProductService());
            var actionResult = controller.PostProduct(product);

            for (var i = 0; i < 100; i++)
            {
                var task = new Task(() => new ProductController(new ProductService()).PostProduct(product));
                task.RunSynchronously();
            }
            Assert.True(true);
        }
        [Fact]
        public void Test4()
        {
        }
    }
}
