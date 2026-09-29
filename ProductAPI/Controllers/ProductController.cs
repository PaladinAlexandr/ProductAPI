using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Validation;
using ProductAPI.Models;
using ProductAPI.Service;
using System.ComponentModel.DataAnnotations;
using System.Net;

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

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return productService.GetProducts();
        }

        [HttpGet("{Id:int}")]
        public IActionResult? Get(int Id)
        {
            var product = productService.GetProductId(Id);
            return product.Item1 == HttpStatusCode.OK ? Ok(product) : NotFound();

        }
        [HttpPost]
        public IActionResult PostProduct(Product product) => productService.AddProduct(product) ? CreatedAtAction("Post", "Объект успешно добавлен", product) : BadRequest("Данные не валидны");

        [HttpPut]
        public IActionResult PutProducts(int Id, Product product) => productService.EditProduct(Id, product) ? Ok("Объект успешно изменён") : product == null ? NotFound() : BadRequest();

        [HttpDelete]
        public IActionResult DeleteProduct(int Id) => productService.DeleteProduct(Id) ? Ok("Объект успешно удалён") : NotFound("Товара не существует");


    }
}
