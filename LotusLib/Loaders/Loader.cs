namespace LotusLib.Loaders
{
    public abstract class Loader
    {
        public string LotusPass { get; set; }
        public string Server { get; set; }

        internal Lotus Lotus { get; private set; }

        public delegate void MyEventHandler(object obj, string foo);

        public event MyEventHandler OnMessage;

        protected Loader(string server, string pass)
        {
            Server = server;
            LotusPass = pass;
            Lotus = new Lotus(Server);
        }

        protected void RaseNewMessage(string msg)
        {
            OnMessage?.Invoke(this, msg);
        }
    }
}
