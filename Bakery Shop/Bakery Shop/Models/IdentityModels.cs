using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bakery_Shop.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Telephone { get; set; }
        public string Name { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet <Product> products { get; set; }
        public DbSet <Category> categories { get; set; }
        public DbSet <Order> Orders { get; set; }
        public DbSet <OrderDetails> OrderDetails { get; set; }
        public DbSet <Driver> Drivers { get; set; }
        public DbSet <CustomOrder> CustomOrders { get; set; }
        public DbSet<Deco> Decos { get; set; }
        public DbSet<Decobooking> Decobookings { get; set; }
        public DbSet<Decopiece> Decopieces { get; set; }
        public DbSet<Orderpiece> Orderpieces { get; set; }
        public DbSet<Cancelation> Cancelations { get; set; }
        public DbSet<Ordercancelation> Ordercancelations { get; set; }
        public DbSet<ImageStore> ImageStores { get; set; }

    }
}