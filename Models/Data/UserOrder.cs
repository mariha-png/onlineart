using System.ComponentModel.DataAnnotations;

namespace Online_Art_Gallery.Models.Data
{
    public class UserOrder
    {
        [Key]
        public int Id { get; set; }

        // Address Info
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }

        // One-to-Many with OrderItems
        public List<OrderItem> Items { get; set; }
    }

}
