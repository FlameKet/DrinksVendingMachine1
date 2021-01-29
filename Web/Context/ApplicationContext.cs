using Microsoft.EntityFrameworkCore;
using Web.Application;

namespace Web.Context
{
    public class ApplicationContext: DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<MachineStack<Drink>> MachineStackDrinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Owned<Drink>();
            //modelBuilder.Entity<MachineStack<Drink>>().HasKey(u => u.Entity.Id);//.HasIndex(u => new {u.Name, u.Volume}).IsUnique();
            modelBuilder.Owned<Coin>();
        }
    }
}
