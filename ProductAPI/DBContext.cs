using ProductAPI.Controllers;

namespace ProductAPI
{
    public class DBContext
    {
        public IEnumerable<Product> Products;

        public DBContext()
        {
            Products = new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Name = "Test",
                    Price = 1000,
                    AmountBox = 10,
                },
                new Product
                {
                    Id = 2,
                    Name = "Test2",
                    Price = 2000,
                    AmountBox = 20,
                },new Product
                {
                    Id = 3,
                    Name = "Test3",
                       Price = 3000,
                    AmountBox = 30,
                }
            };
        }
    }
}
