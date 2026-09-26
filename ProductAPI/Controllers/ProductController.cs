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

        [HttpGet("api/products")]
        public IEnumerable<Product> Get()
        {
            return productService.GetProducts();
        }

        [HttpGet("api/products/{Id}")]
        public Product? Get(int Id)
        {
            return productService.GetProductId(Id);
        }
        [HttpPost(Name = "PostProduct")]
        public IActionResult PostProduct(Product product) => productService.AddProduct(product) ? CreatedAtAction("Объект успешно добавлен", product) : BadRequest("Данные не валидны");

        [HttpPut(Name = "PutProduct")]
        public IActionResult PutProducts(int Id, Product product) => productService.EditProduct(Id, product) ? Ok("Объект успешно изменён") : BadRequest("Данные не валидны");

        [HttpDelete(Name = "DeleteProduct")]
        public IActionResult DeleteProduct(int Id) => productService.DeleteProduct(Id) ? Ok("Объект успешно удалён") : NotFound("Товара не существует");


    }
}
