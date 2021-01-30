using System;
using System.Collections.Generic;
using System.Linq;
using Web.Context;

namespace Web.Application
{
    public class RepositoryCoin
    {
        private readonly IApplicationContext _db;
        public RepositoryCoin(IApplicationContext db) { _db = db; }

        public IReadOnlyCollection<MachineStack<Coin>> Get() => _db.MachineStackCoins.ToList();

        public MachineStack<Coin> Get(Coin entity) => _db.MachineStackCoins.FirstOrDefault(x => x.Entity.Par.Equals(entity.Par));
        public MachineStack<Coin> Get(int id) => _db.MachineStackCoins.FirstOrDefault(x => x.Id == id);
        public MachineStack<Coin> Add(Coin entity, int quantity)
        {
            var machineStackCoin = Get(entity); 
            if (machineStackCoin == null)
                _db.MachineStackCoins.Add(new MachineStack<Coin>(entity, quantity));
            else
                machineStackCoin.Add(quantity);

            _db.SaveChanges();
            return Get(entity);
        }
        public MachineStack<Coin> ChangeBlocking(Coin entity, bool blocking)
        {
            var machineStackCoin = Get(entity);
            machineStackCoin?.Entity.ChangeBlocking(blocking);
            _db.SaveChanges();
            return machineStackCoin;
        }
        public MachineStack<Coin> GiveOut(Coin entity, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            
            var machineStackCoin = Get(entity);
            if (machineStackCoin == null) throw new Exception("Не найден");
            machineStackCoin.GiveOut(quantity);

            _db.SaveChanges();
            return Get(entity);
        }
    }
}
