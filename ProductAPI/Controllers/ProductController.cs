using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Service;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("/api/products")]//ProductController
    public class ProductController : Controller
    {
        IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("/products")]
        public IEnumerable<Product> Get()
        {
            return ProductService.Products;
        }

        [HttpGet("/product/{Id}")]
        public Product? Get(int Id)
        {
            return ProductService.Products.Where(p => p.Id == Id).FirstOrDefault();
        }
        [HttpPost(Name = "PostProduct")]
        public IActionResult PostProduct(Product product) => productService.AddProduct(product) ? BadRequest("Данные не валидны") : Created("Объект успешно добавлен",product);

        [HttpPut(Name = "PutProduct")]
        public IActionResult PutProducts(int Id, Product product) => productService.EditProduct(Id, product) ? BadRequest("Данные не валидны") : Ok("Объект успешно изменён");
  
        [HttpDelete(Name = "DeleteProduct")]
        public IActionResult DeleteProduct(int Id) => productService.DeleteProduct(Id) ? NotFound("Товара не существует") : Ok("Объект успешно удалён");
        
      
    }
}
