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
            var result = productService.GetProductId(Id);
            IActionResult actionResult = result.Type switch
            {
                TypeError.Success => Ok(result.Value),
                TypeError.ValidationError => BadRequest(result.Errors),
                TypeError.NotFound => NotFound(result.Value),
                TypeError.Conflict => Conflict(result.Value),
            };
            return actionResult;

        }
        [HttpPost]
        public IActionResult PostProduct(Product product)
        {
            var result = productService.AddProduct(product);
            IActionResult actionResult = result.Type switch
            {
                TypeError.Success => CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value),
                TypeError.ValidationError => BadRequest(result.Errors),
                TypeError.NotFound => NotFound(result.Value),
                TypeError.Conflict => Conflict(result.Value),
            };
            return actionResult;
        }

        [HttpPut("{id:int}")]
        public IActionResult PutProducts(int Id, Product product)
        {
            var result = productService.EditProduct(Id, product);
            IActionResult actionResult = result.Type switch
            {
                TypeError.Success => Ok(result.Value),
                TypeError.ValidationError => BadRequest(result.Errors),
                TypeError.NotFound => NotFound(result.Value),
                TypeError.Conflict => Conflict(result.Value),
            };
            return actionResult;
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int Id)
        {
            var result = productService.DeleteProduct(Id);
            IActionResult actionResult = result.Type switch
            {
                TypeError.Success => Ok(result.Value),
                TypeError.ValidationError => BadRequest(result.Errors),
                TypeError.NotFound => NotFound(result.Value),
                TypeError.Conflict => Conflict(result.Value),
            };
            return actionResult;
        }


    }
}
