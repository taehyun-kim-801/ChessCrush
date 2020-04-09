using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ChessCrush.Game
{
    public class NetworkHelper
    {
        private readonly string serverIPString;
        private readonly int port;
        private readonly int maxBufferSize = 1024;
        
        private Socket socket;
        public bool socketConnected { get { return socket.Connected; } }

        public NetworkHelper() 
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 5000;
            socket.ReceiveTimeout = 5000;
            socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            Task.Run(() => socket.Connect(IPAddress.Parse(serverIPString), port));
        }

        public bool ParticipateGame()
        {
            OutputMemoryStream oms = new OutputMemoryStream();
            oms.Write(true);
            oms.Write(Director.instance.playerName);
            socket.Send(oms.buffer);

            byte[] receiveBuffer = new byte[maxBufferSize];
            try
            {
                int receiveBytes = socket.Receive(receiveBuffer);
                InputMemoryStream ims = new InputMemoryStream(receiveBuffer);
                ims.Read(out int roomId);
                ims.Read(out string playerName);
                return true;
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }
    }
}