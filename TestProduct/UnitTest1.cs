using Microsoft.AspNetCore.Components;
using ProductAPI.Controllers;
using ProductAPI.Service;

namespace TestProduct
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ProductController Test = new(new ProductService());
            var GetProduct = Test.Get();
            Assert.True(GetProduct.Count() == 3);
        }
        [Fact]
        public void Test2()
        {
            ProductController Test = new(new ProductService());
            Test.PostProduct(new ProductAPI.Models.Product
            {
                Name = "___",
                Price = 100,
                Stock = 10
            }
            );
            var GetProduct = Test.Get();
            Assert.True(GetProduct.Count() == 4);
        }
        [Fact]
        public void Test3()
        {
            ProductService Test = new ProductService();
            Assert.False(Test.EditProduct(1, null));
        }
    }
}
