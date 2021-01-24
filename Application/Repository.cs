using System;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class Repository<T> where T : IEquatable<T>
    {
        private readonly ICollection<Stack<T>> _stacks;
        public Repository(IEnumerable<Stack<T>> stackDrinks)
        {
            _stacks = new List<Stack<T>>();
            foreach (var stackDrink in stackDrinks)
                _stacks.Add(stackDrink);
        }

        public IReadOnlyCollection<Stack<T>> Get() => (IReadOnlyCollection<Stack<T>>) _stacks;
        public Stack<T> Get(T entity) => _stacks.FirstOrDefault(x => x.Entity.Equals(entity));
        public Stack<T> Add(T entity, int quantity)
        {
            var stack = Get(entity); 
            if (stack == null)
                _stacks.Add(new Stack<T>(entity, quantity));
            else
                stack.Add(quantity);

            return Get(entity);
        }
        public Stack<T> GiveOut(T entity, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            
            var stack = Get(entity);
            if (stack == null) throw new Exception("Не найден");
            stack.GiveOut(quantity);

            return Get(entity);
        }
    }
}
