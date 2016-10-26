using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Server
    {
        private ServerSocket sHost;

        public Server()
        {
            sHost = new ServerSocket();
            sHost.StartServer("127.0.0.1", 6556);
        }
    }

    class ServerSocket
    {
        private Socket _serverSocket;
        private Socket _clientSocket;

        private byte[] _buffer;

        public void StartServer(string ip, int port)
        {
            try
            {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                _serverSocket.Listen(50);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}.", e);
            }
        }

        private void AcceptCallBack(IAsyncResult AR)
        {
            try
            {
                Socket clientSocket = _serverSocket.EndAccept(AR);
                _buffer = new byte[1024];
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
                Accept();
                /* Myk's code
                _clientSocket = _serverSocket.EndAccept(AR);
                _buffer = new byte[_clientSocket.ReceiveBufferSize];
                _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), null);//*/
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}.", e);
            }
        }

        public void Accept()
        {
            _serverSocket.BeginAccept(AcceptCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult AR)
        {
            Socket clientSocket = AR.AsyncState as Socket;
            int bufferSize = clientSocket.EndReceive(AR);
            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, packet.Length);

            PacketHandler.Handle(packet, clientSocket);

            _buffer = new byte[1024];
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);

            /* Myk's code
            try
            {
                string text = Encoding.ASCII.GetString(_buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}.", e);
            }//*/
        }
    }
}
