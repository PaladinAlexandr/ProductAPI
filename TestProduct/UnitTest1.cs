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
            Product product;
            var service = new ProductService();
            int index = service.GetProducts().Max(x => x.Id);
            for (var i = 0; i < 100; i++)
            {
                product = new Product()
                {
                    Name = "Test",
                    Price = new Random().Next(1, 1000),
                    Stock = new Random().Next(0, 1000)
                };
                var task = Task.Run (() => new ProductController(service).PostProduct(product));
            
            }
            int newIndex =service.GetProducts().Max(x => x.Id);
            Assert.Equal(newIndex,index + 100) ;
        }
        [Fact]
        public void Test4()
        {
        }
    }
}
