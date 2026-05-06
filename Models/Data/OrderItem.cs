using System.ComponentModel.DataAnnotations;

namespace Online_Art_Gallery.Models.Data
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        // Foreign Key
        public int UserOrderId { get; set; }
        public UserOrder UserOrder { get; set; }
    }

}
