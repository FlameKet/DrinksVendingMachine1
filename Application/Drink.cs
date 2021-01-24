using System;
using System.Diagnostics.CodeAnalysis;

namespace Application
{
    public class Drink : IEquatable<Drink>
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
        public void ReEevaluate(int price) { _price = price; }

        public bool Equals([AllowNull] Drink other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            return  other.Name == Name || other.Volume == Volume;
        }
    }
}
