using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Art_Gallery.Models.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
