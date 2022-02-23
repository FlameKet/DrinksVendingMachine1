using Web.Application;

namespace Web.Models
{
    public class CoinModel
    {
        public int Id { get; set; }
        public EnumCoin Par { get;  set; }
        public bool Blocking { get; set; }
        public int Quantity { get; set; }
    }
}
