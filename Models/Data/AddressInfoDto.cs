using System.ComponentModel.DataAnnotations;

namespace Online_Art_Gallery.Models.Data
{
    public class AddressInfoDto
    {
        //[Key]
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
