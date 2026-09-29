using Microsoft.AspNetCore.Http.HttpResults;
using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
namespace ProductAPI.Service
{

    public interface IProductService
    {
        
        public IEnumerable<Product> GetProducts();
        public bool AddProduct(Product product);
        public bool DeleteProduct(int Id);
        public bool EditProduct(int Id, Product product);
        public (HttpStatusCode, Product?) GetProductId(int Id);
    }
    public class ProductService : IProductService
    {
        public (HttpStatusCode, Product?) GetProductId(int Id)
        {
            var product = Products.FirstOrDefault(x => x.Id == Id);
            if (product == null)
                return (HttpStatusCode.OK, product);
            else
                return (HttpStatusCode.NotFound, product);

        }

        public class ProductValidationResult
        {
            public bool Success { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }
        public ProductValidationResult ProductValidate(Product product)
        {
            var Result = new ProductValidationResult();
            if (string.IsNullOrWhiteSpace(product.Name)) Result.Errors.Add("Имя не должно быть пустым");
            if (product == null) Result.Errors.Add("Объект пустой");
            if (product.Price <= 0)
            {
                Result.Errors.Add("Цена должа быть больше нуля");

            }
            if (product.Stock < 0)
            {
                Result.Errors.Add("Количество на складе не должно быть отрицательным");
            }
            Result.Success = !Result.Errors.Any();
            return Result;
        }
        public IEnumerable<Product> GetProducts()
        {
            return Products;
        }

        public bool AddProduct(Product product)
        {
            Lock @lock = new();
            lock (@lock)
            {
                if (ProductValidate(product).Success)
                {
                    product.Id = Products.Max(x => x.Id + 1);
                    Products.Add(product);

                    return true;
                }
                return false;
            }
        }

        public bool DeleteProduct(int Id)
        {
            Lock @lock = new();
            lock (@lock)
            {
                var ProductDelete = Products.Find(x => x.Id == Id);
                if (ProductDelete != null)
                {
                    Products.Remove(ProductDelete);
                    return true;
                }
                return false;
            }

        }

        public bool EditProduct(int Id, Product product)
        {
            Lock @lock = new();
            lock (@lock)
            {

                var ProductEdit = Products.Where(p => p.Id == Id).FirstOrDefault();
                if (ProductEdit != null && ProductValidate(product).Success)
                {
                    ProductEdit.Name = product.Name;
                    ProductEdit.Price = product.Price;
                    ProductEdit.Stock = product.Stock;
                    return true;
                }
                return false;
            }
        }

        private readonly List<Product> Products = new()
        {
                     new Product
                {
                    Id = 1,
                    Name = "Test",
                    Price = 1000,
                    Stock = 10,
                },
                new Product
                {
                    Id = 2,
                    Name = "Test2",
                    Price = 2000,
                    Stock = 20,
                },new Product
                {
                    Id = 3,
                    Name = "Test3",
                       Price = 3000,
                    Stock = 30,
                }
        };


    }
}
