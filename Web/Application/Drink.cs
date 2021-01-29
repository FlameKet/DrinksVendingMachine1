using System;

namespace Web.Application
{
    public class Drink
    {
        private int _price;
        public Drink(string name, string image, int volume, int price) {
            if (volume <= 0) throw new ArgumentOutOfRangeException(nameof(volume));
            if (price <= 0) throw new ArgumentOutOfRangeException(nameof(price));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Image = image;
            Volume = volume;
            _price = price;
        }

        public string Name { get; protected set; }
        public string Image { get; protected set; }
        public int Volume { get; protected set; }
        public int Price
        {
            get => _price;
            set=> _price = value;
        }
        public void ChangePrice(int price) { _price = price; }
    }
}
