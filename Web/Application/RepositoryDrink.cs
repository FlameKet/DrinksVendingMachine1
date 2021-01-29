using System;
using System.Collections.Generic;
using System.Linq;
using Web.Context;

namespace Web.Application
{
    public class RepositoryDrink
    {
        private readonly ApplicationContext _db;
        public RepositoryDrink(ApplicationContext db) { _db = db; }

        public IReadOnlyCollection<MachineStack<Drink>> Get() => _db.MachineStackDrinks.ToList();

        public MachineStack<Drink> Get(Drink entity) => _db.MachineStackDrinks.FirstOrDefault(x => x.Entity.Name == entity.Name && x.Entity.Volume == entity.Volume);
        public MachineStack<Drink> Get(int id) => _db.MachineStackDrinks.FirstOrDefault(x => x.Id == id);
        public MachineStack<Drink> Add(Drink entity, int quantity)
        {
            var machineStackDrink = Get(entity); 
            if (machineStackDrink == null)
                _db.MachineStackDrinks.Add(new MachineStack<Drink>(entity, quantity));
            else
            {
                machineStackDrink.Add(quantity);
                if (entity.Price != machineStackDrink.Entity.Price)
                    machineStackDrink.Entity.ChangePrice(entity.Price);
            }

            _db.SaveChanges();
            return Get(entity);
        }
        public MachineStack<Drink> ChangePrice(Drink entity, int price)
        {
            var machineStackDrink = Get(entity);
            machineStackDrink?.Entity.ChangePrice(price);
            _db.SaveChanges();
            return machineStackDrink;
        }
        public MachineStack<Drink> GiveOut(Drink entity, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            
            var machineStackDrink = Get(entity);
            if (machineStackDrink == null) throw new Exception("Не найден");
            machineStackDrink.GiveOut(quantity);

            _db.SaveChanges();
            return Get(entity);
        }

        public void Delete(Drink entity)
        {
            var machineStackDrink = Get(entity);
            if (machineStackDrink == null) throw new Exception("Не найден");
            _db.MachineStackDrinks.Remove(machineStackDrink);

            _db.SaveChanges();
        }
    }
}
