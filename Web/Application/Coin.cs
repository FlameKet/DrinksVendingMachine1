namespace Web.Application
{
    public enum EnumCoin { One=1, Two=2, Five=5, Ten=10 }
    public class Coin
    {
        private bool _blocking;

        public Coin(EnumCoin par)
        {
            Par = par;
            _blocking = false;
        }

        public EnumCoin Par { get; protected set; }
        public bool Blocking
        {
            get => _blocking;
            set => _blocking = value;
        }
        public void ChangeBlocking(bool blocking) { _blocking = blocking; }
    }
}
