using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Online_Art_Gallery.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Optional: only needed if you're manually querying AppUser table
        public DbSet<AppUser> AppUser { get; set; }

        public DbSet<ContactFormViewModel> ContactFormsViewModel { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<CartItem> cartItems { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
