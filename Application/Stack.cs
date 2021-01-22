using System;

namespace Application
{
    public class Stack<T>
    {
        private int _quantity;
        public Stack(T entity, int quantity)
        {
            Entity = entity;
            _quantity = quantity;
        }
        public T Entity { get; }
        public int Quantity => _quantity;
        public void Add(int quantity)
        {
            if (quantity <= 0) 
                if (_quantity+quantity < 0)
                    throw new ArgumentOutOfRangeException(nameof(quantity));
            _quantity += quantity;
        }
        public bool MayGiveOut(int quantity) => _quantity >= quantity;
        public void GiveOut(int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (!MayGiveOut(quantity)) throw new Exception("Нужное количество не найдено");
            _quantity -= quantity;
        }
    }
}
