using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;
namespace ProductAPI.Service
{

    public interface IProductService
    {
        public IEnumerable<Product> GetProducts();
        public bool AddProduct(Product product);
        public bool DeleteProduct(int Id);
        public bool EditProduct(int Id, Product product);
    }
    public class ProductService : IProductService
    {
        public ValidationResult ProductValidate(Product product)
        {
            if (product == null) return new ValidationResult("Объект пустой");
            if (product.Price <= 0)
            {
                return new ValidationResult("Цена должа быть больше нуля");

            }
            if (product.Stock <= 0)
            {
                return new ValidationResult("Количество на складе должно быть больше нуля");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        public IEnumerable<Product> GetProducts()
        {
            return Products;
        }

        public bool AddProduct(Product product)
        {
            if (ProductValidate(product) == ValidationResult.Success)
            {
                Products.Add(product);
                return true;
            }
            return false;
        }

        public bool DeleteProduct(int Id)
        {
            var ProductDelete = Products.Find(x => x.Id == Id);
            if (ProductDelete != null)
            {
                Products.Remove(ProductDelete);
                return true;
            }
            return false;
        }

        public bool EditProduct(int Id, Product product)
        {
            var ProductEdit = Products.Where(p => p.Id == Id).FirstOrDefault();
            if (ProductEdit != null && ProductValidate(product) == ValidationResult.Success)
            {
                ProductService.Products[Id] = product;
                return true;
            }
            return false;
        }

        public static List<Product> Products { get; set; } = new()
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
