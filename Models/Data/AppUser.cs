using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Online_Art_Gallery.Models.Data
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Column(TypeName = "Varchar(50)")]
        public string? Name { get; set; }

        [Required]
        [Column(TypeName = "Varchar(50)")]
        public string? StreetAddress { get; set; }

        [Required]
        [Column(TypeName = "Varchar(50)")]
        public string? PostalCode { get; set; }

        [Required]
        [Column(TypeName = "Varchar(50)")]
        public string? Country { get; set; }

        [Required]
        [Column(TypeName = "Varchar(50)")]
        public string? City { get; set; }

        [Column(TypeName = "Varchar(100)")]
        public string? ProfileImage { get; set; } // ✅ Added this line
    }
}
