﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

/*
 * 
 * Created on 19-09-2016
 * Last edit on 21-09-2016
 * 
*/

namespace Server
{
    public partial class ServerForm : Form
    {
        private Socket _serverSocket;
        private List<Socket> _clientSockets = new List<Socket>();

        private byte[] _buffer;

        public ServerForm()
        {
            InitializeComponent();
            /*
            List<string> testReceiveList = new List<string>();
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("ClientName-2016-10-29-15-07-0.xml");
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(List<string>);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        testReceiveList = (List<string>)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
            foreach(string s in testReceiveList)
                Console.WriteLine(s);
            //*/
        }

        // Starts up a server with the given IP and Port number.
        private void StartServer(IPAddress ip, int port)
        {
            try
            {
                //Init the new socket 
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //binds the socket with the ip and port
                _serverSocket.Bind(new IPEndPoint(ip, port));
                //Amount of connections there can be in the queue
                _serverSocket.Listen(10);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception e)
            {
                MessageBox.Show("StartServer: " + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Accepts the client connection with the service
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = _serverSocket.EndAccept(ar);
                _buffer = new byte[1024];
                clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
                Accept();
                AppendToTextBox("Client connected!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}.", e);
            }
        }

        public void Accept()
        {
            _serverSocket.BeginAccept(AcceptCallback, null);
        }

        //Does something with the data that was received, change this to change the functionality
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            int bufferSize = clientSocket.EndReceive(ar);
            byte[] packet = new byte[bufferSize];
            Array.Copy(_buffer, packet, packet.Length);

            PacketHandler.Handle(packet, clientSocket);

            _buffer = new byte[1024];
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
            
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Client Disconnected. {0}", e);
            }

        }

        //Text to textbox method, adds text to it instead of overwritting.
        private void AppendToTextBox(string text)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                textBoxReceived.Text += "\r\n" + text + "\r\n";
            });
            this.Invoke(invoker);
        }

        //Does stuff when the create server is pressed.
        private void btnCreateServer_Click(object sender, EventArgs e)
        {
            AppendToTextBox("Server created.");
            //Calls a method that starts up a server with the IP and Port written by the server user.
            StartServer(IPAddress.Parse(textBoxIP.Text), int.Parse(textBoxPort.Text));
            //Enables and disables certain buttons.
            btnCreateServer.Enabled = false;
            btnCloseServer.Enabled = true;
            btnClear.Enabled = true;
            btnSend.Enabled = true;
        }

        //Press close for closure press f for respect
        private void btnCloseServer_Click(object sender, EventArgs e)
        {
            try
            {
                //Closes the server socket and sets the client socket to null, enables the same port to be used and gets rid
                //any clients connected.
                _serverSocket.Close();
                foreach (Socket s in _clientSockets)
                {
                    s.Close();
                }
                AppendToTextBox("Server Closed");
                btnCreateServer.Enabled = true;
                btnCloseServer.Enabled = false;
                btnSend.Enabled = false;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Null Reference Execption woo! {0}", ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnCloseServer: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //Makes the clear button clear the two textboxes
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBoxReceived.Clear();
            textBoxSend.Clear();
        }
    }
}
