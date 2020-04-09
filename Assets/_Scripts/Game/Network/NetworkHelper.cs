using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChessCrush.Game
{
    public class NetworkHelper
    {
        private static string serverIPString;
        private static int port;

        private Socket socket;
        public bool socketConnected { get { return socket.Connected; } }

        public NetworkHelper() 
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            socket.Connect(IPAddress.Parse(serverIPString), port);
        }
    }
}