using System;

namespace Web.Application
{
    public class MachineStack<T>
    {
        private MachineStack() { }
        private int _quantity;
        public MachineStack(T entity, int quantity)
        {
            Entity = entity;
            _quantity = quantity;
        }

        public int Id { get; set; }
        public  T Entity { get; protected set; }
        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }
        public void Add(int quantity)
        {
            if (quantity == 0) return;
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
