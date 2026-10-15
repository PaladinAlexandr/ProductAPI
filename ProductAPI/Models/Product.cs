namespace ProductAPI.Models
{
    public class Product
    {
        public static int IdAll { get;  set; } = 0;
        public Product()
        {
            IdAll++;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
