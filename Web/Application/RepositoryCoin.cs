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
        public MachineStack<Coin> AddPreviously(Coin entity, int quantity)
        {
            var machineStackCoin = Get(entity);
            if (machineStackCoin == null)
                _db.MachineStackCoins.Add(new MachineStack<Coin>(entity, quantity));
            else
                machineStackCoin.Add(quantity);

            //_db.SaveChanges();
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

        public List<MachineStack<Coin>> ReturnBalance(int balance)
        {
            return CalculateChange(balance);
        }

        private List<MachineStack<Coin>> CalculateChange(int balance)
        {
            var tuple = CalculateChange(EnumCoin.Ten, balance, new List<MachineStack<Coin>>());
            tuple = CalculateChange(EnumCoin.Five, tuple.Item1, tuple.Item2);
            tuple = CalculateChange(EnumCoin.Two, tuple.Item1, tuple.Item2);
            tuple = CalculateChange(EnumCoin.One, tuple.Item1, tuple.Item2);

            if (tuple.Item1 > 0) throw new Exception("Вернуть сдачу невозможно.");
            return tuple.Item2;
        }

        private (int, List<MachineStack<Coin>>) CalculateChange(EnumCoin par, int balance, List<MachineStack<Coin>> coins)
        {
            int modulo;
            if (balance % (int)par < balance)
            {
                var quantity = balance / (int)par;
                var coin = Get(new Coin(par));
                var quantityInStock = coin.Quantity;
                if (quantityInStock < quantity) quantity = quantityInStock;
                if (!coin.Entity.Blocking)
                {
                    coins.Add(new MachineStack<Coin>(new Coin(par), quantity));
                    modulo = balance % (int) par;
                    balance = modulo;
                }
                else
                    coins.Add(new MachineStack<Coin>(new Coin(par){Blocking = true}, 0));
            }
            else
                coins.Add(new MachineStack<Coin>(new Coin(par), 0));

            return (balance, coins);
        }
    }
}
