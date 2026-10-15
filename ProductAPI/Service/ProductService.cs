using Microsoft.AspNetCore.Http.HttpResults;
using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
namespace ProductAPI.Service
{

    public interface IProductService
    {

        public IEnumerable<Product> GetProducts();
        public Result<Product> AddProduct(Product product);
        public Result<Product> DeleteProduct(int Id);
        public Result<Product> EditProduct(int Id, Product product);
        public Result<Product> GetProductId(int Id);
    }
    public enum TypeError
    {
        Success,
        ValidationError,
        NotFound,
        Conflict
    }
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Value { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public TypeError Type { get; set; }

        public Result(bool Success, T value, string Error, TypeError Type)
        {
            this.Success = Success;
            this.Value = value;
            if (Error != null)
                Errors.Add(Error);
            this.Type = Type;
        }
        public Result(bool Success, T value, TypeError Type)
        {
            this.Success = Success;
            this.Value = value;
            this.Type = Type;
        }
        public Result() { }


    }
    public class ProductService : IProductService
    {
        private readonly Lock _sync = new Lock();

        public Result<Product> GetProductId(int Id)
        {
            lock (_sync)
            {
                var product = Products.FirstOrDefault(x => x.Id == Id);
                if (product == null)
                    return new Result<Product>(false, product, "товар не найден", TypeError.NotFound);
                else
                    return new Result<Product>(true, product, TypeError.Success);
            }


        }


        public Result<Product> ProductValidate(Product product)
        {


            var Result = new Result<Product>();
            Result.Value = product;
            Result.Success = false;
            Result.Type = TypeError.ValidationError;
            if (product == null)
                Result.Errors.Add("Объект пустой");
            if (string.IsNullOrWhiteSpace(product?.Name))
                Result.Errors.Add("Имя не должно быть пустым");
            if (product?.Price <= 0)
                Result.Errors.Add("Цена должа быть больше нуля");
            if (product?.Stock < 0)
                Result.Errors.Add("Количество на складе не должно быть отрицательным");


            Result.Success = !Result.Errors.Any();
            if (Result.Success)
                Result.Type = TypeError.Success;
            return Result;
        }
        public IEnumerable<Product> GetProducts()
        {
            lock (_sync)
                return Products.ToArray();
        }

        public Result<Product> AddProduct(Product product)
        {

            lock (_sync)
            {
                var result = ProductValidate(product);
                if (result.Success)
                {
                    lock (_sync)
                    {
                       
                        product.Id = ++Product.IdAll;
                        Products.Add(product);
                    }
                }
                return result;
            }
        }

        public Result<Product> DeleteProduct(int Id)
        {

            lock (_sync)
            {
                var ProductDelete = Products.Find(x => x.Id == Id);
                if (ProductDelete == null)
                    return new Result<Product>(false, ProductDelete, "Пустой объект", TypeError.NotFound);

                Products.Remove(ProductDelete);
                return new Result<Product>(true, ProductDelete, TypeError.Success);
            }

        }

        public Result<Product> EditProduct(int Id, Product product)
        {
            Result<Product> result = new();
            result.Value = product;
            lock (_sync)
            {

                var ProductEdit = Products.Where(p => p.Id == Id).FirstOrDefault();
                if (ProductEdit == null)
                {
                    result = new(false, product, "Объект не найден", TypeError.NotFound);
                    return result;
                }
                result = ProductValidate(product);
                if (result.Success == false) return result;


                ProductEdit.Name = product.Name;
                ProductEdit.Price = product.Price;
                ProductEdit.Stock = product.Stock;
                result = new(true, product, TypeError.Success);
                return result;

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
