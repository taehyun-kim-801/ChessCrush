using ChessCrush.OperationResultCode;
using System;
using System.IO;
using System.Net;
using System.Net.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ChessCrush.Game
{
    public class NetworkHelper
    {
        private readonly string serverIPString;
        private readonly string portString;
        private readonly int maxBufferSize = 1024;
        
        private Socket socket;
        public bool socketConnected { get { return socket.Connected; } }

        public NetworkHelper() 
        {
            var jsonText = File.ReadAllText("Assets/Data/NetworkSettings.json");
            JsonTextParser parser = new JsonTextParser();
            var jsonObjectCollection = parser.Parse(jsonText) as JsonObjectCollection;
            serverIPString = (string)jsonObjectCollection["ServerIP"].GetValue();
            portString = (string)jsonObjectCollection["ServerPort"].GetValue();

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = 5000;
            socket.ReceiveTimeout = 5000;
            socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            Task.Run(() => socket.Connect(IPAddress.Parse(serverIPString), Convert.ToInt32(portString)));
        }

        public int SignIn(string id,string password)
        {
            OutputMemoryStream oms = new OutputMemoryStream();
            oms.Write((int)OperationCode.SignIn);
            oms.Write(id);
            oms.Write(password);
            socket.Send(oms.buffer);

            byte[] receiveBuffer = new byte[maxBufferSize];
            try
            {
                int receiveBytes = socket.Receive(receiveBuffer);
                InputMemoryStream ims = new InputMemoryStream(receiveBuffer);
                ims.Read(out int result);
                return result;
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                return -1;
            }
        }

        public SignUpCode SignUp(string newId,string newPassword)
        {
            OutputMemoryStream oms = new OutputMemoryStream();
            oms.Write((int)OperationCode.SignUp);
            oms.Write(newId);
            oms.Write(newPassword);
            socket.Send(oms.buffer);

            byte[] receiveBuffer = new byte[maxBufferSize];
            try
            {
                int receiveBytes = socket.Receive(receiveBuffer);
                InputMemoryStream ims = new InputMemoryStream(receiveBuffer);
                ims.Read(out int result);
                return (SignUpCode)result;
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
                return SignUpCode.Etc;
            }
        }

        public int ParticipateGame()
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
                return receiveBytes;
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
                return -1;
            }
        }
    }
}