using System.ComponentModel.DataAnnotations;

namespace Online_Art_Gallery.Models.Data
{
    public class OrderSummaryViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<CartItem> CartItems { get; set; }
    }

}
