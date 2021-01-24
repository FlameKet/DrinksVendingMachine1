using Application;
using Microsoft.EntityFrameworkCore;

namespace Web.Context
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Stack<Drink>> StackDrinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<Drink>();
            modelBuilder.Owned<Coin>();
        }
    }
}
