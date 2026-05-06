using System.ComponentModel.DataAnnotations;

namespace Online_Art_Gallery.Models.Data
{
    public class CartItem
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }  // ✅ Quantity
        public decimal TotalPrice => Price * Quantity; // ✅ Computed Total
    }

}
