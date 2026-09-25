using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        DBContext DB = new DBContext();


        [HttpGet(Name = "GetProduct")]
        public IEnumerable<Product> Get()
        {
            return DB.Products;
        }

        [HttpGet(Name = "GetProductId")]
        public Product? Get(int Id)
        {
            return DB.Products.Where(p => p.Id == Id).FirstOrDefault();
        }
        [HttpPost(Name = "PostProduct")]
        public HttpResponseMessage PostProduct(Product product)
        {
            if (ProductValidate(product) == Results.Ok())
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("Объект успешно создан")
                };
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Данные не валидны")
                };
            }
        }
        [HttpPut(Name = "PutProduct")]
        public HttpResponseMessage PutProducts(int Id, Product product)
        {
           var ProductEdit = DB.Products.Where(p=>p.Id==Id).FirstOrDefault();
            if(ProductEdit == null || ProductValidate(product) == Results.BadRequest())
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Данные не валидны")
                };
            }
            else
            {
                DB.Products.ToArray()[Id] = product;
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("Объект успешно изменён")
                };
            }
        }
        [HttpDelete(Name = "DeleteProduct")]
        public HttpResponseMessage DeleteProduct(int Id)
        {
           var ProductEdit = DB.Products.Where(p=>p.Id==Id).FirstOrDefault();
            if(ProductEdit == null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Товара не существует")
                };
            }
            else
            {
                DB.Products.ToList().Remove(ProductEdit);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("Объект успешно удалён")
                };
            }
        }
        public ActionResult ProductValidate(Product product)
        {
            if (product.Price <= 0)
            {
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    ["Price"] = ["Цена должа быть больше нуля"]
                }));
            }
            if (product.AmountBox <= 0)
            {
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    ["AmountBox"] = ["Количество на складе должно быть больше нуля"]
                }));
            }
            else
            {
                return Ok();
            }
        }
    }
}
