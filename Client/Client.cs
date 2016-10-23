using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BikeAppA3;

namespace Client
{
    class Client
    {
        public Bike bike;
        public ClientSocket conn;

        public Client()
        {
            conn = new ClientSocket();
            conn.Connect("127.0.0.1", 6556);
            bike = new BikeConnection();
            if(bike.Connected)
            {

            }
            else
            {
                Console.WriteLine("Starting Simulation!");
                bike = new Sim();
            }
        }
    }

    class ClientSocket
    {
        public Socket _socket;
        private byte[] _buffer;

        public ClientSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), ConnectCallBack, null);
        }

        private void ConnectCallBack(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                Console.WriteLine("Connected!");
                _buffer = new byte[1024];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallBack, null);

                #region Initial Packet

                #endregion
            }
            else
            {
                Console.WriteLine("Could not connect to server! Exiting now..");
                Application.Exit();
            }
        }

        private void ReceivedCallBack(IAsyncResult result)
        {
            Socket clientSocket = result.AsyncState as Socket;
            int bufLength = _socket.EndReceive(result);
            byte[] packet = new byte[bufLength];
            Array.Copy(_buffer, packet, packet.Length);

            // use packet..
            PacketHandler.Handle(packet, clientSocket);

            // do it again
            _buffer = new byte[1024];
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceivedCallBack, null);
        }

        public void sendData(byte[] data)
        {
            try
            {
                _socket.Send(data);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error: {0}",e.Message);
            }
        }

    }
    
}