namespace Web.Application
{
    public enum EnumCoin { One=1, Two=2, Five=5, Ten=10 }
    public class Coin
    {
        public int Id { get; protected set; }
        public EnumCoin Par { get; set; }
        public bool Blocking { get; set; }
        public int Quantity { get; set; }
    }
}
