using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Application;

namespace Web.Context
{
    public interface IApplicationContext
    {
        DbSet<MachineStack<Drink>> MachineStackDrinks { get; set; }
        DbSet<MachineStack<Coin>> MachineStackCoins { get; set; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
    }
}