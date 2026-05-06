using System.ComponentModel.DataAnnotations;

namespace Online_Art_Gallery.Models.Data
{
    public class ContactFormViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Website { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
