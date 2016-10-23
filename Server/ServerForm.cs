using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                //stops accepting connections?
                Socket socket = _serverSocket.EndAccept(ar);
                _clientSockets.Add(socket);
                AppendToTextBox("A client has connected.");
                //data buffer
                _buffer = new byte[socket.ReceiveBufferSize];
                //Client socket starts to recieve data.
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                //Server socket starts to recieve data.
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("Object disposed Exception hoo! {0}", e);
            }
            catch (Exception e)
            {
                MessageBox.Show("AcceptCallback: " + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Does something with the data that was received, change this to change the functionality
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                int received = socket.EndReceive(ar);
                //Stops it from breaking when nothing has been sent.
                if (received == 0)
                {
                    return;
                }
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);
                //Changes bytes to String
                String text = Encoding.ASCII.GetString(_buffer);
                //Checks if the text contains a certain string
                if (text.Contains("-exit"))
                {
                    socket.Close();
                    _serverSocket.Close();
                    Application.Exit();
                }
                AppendToTextBox("Client says: " + text);
                //Makes the server able to receive again.
                byte[] data = Encoding.ASCII.GetBytes(text);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine("Object Disposed Exception yay! {0}", e);
            }
            catch (Exception e)
            {
                MessageBox.Show("ReceiveCallback: " + e.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Console.WriteLine("Client is gone.");
                //TO-DO find a proper fix
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
