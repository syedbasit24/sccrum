using System.ComponentModel.DataAnnotations.Schema;

namespace sccrumproject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
