using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Client;

namespace Doctor
{
    public partial class Form1 : Form
    {
        private byte[] buffer;
        private Socket socket;
        private int view, clientID;
        private List<List<string>> data = new List<List<string>>();

        public Form1()
        {
            InitializeComponent();
            //ConnectToServer();
            GenerateTable();
        }

        private void GenerateTable()
        {
            view = 0;
            DataTable table = new DataTable();

            table.Columns.Add("Client Name", typeof(string));
            //table.Columns.Add("Date", typeof(string));

            string path = Settings.Data.PATH;

            if (Directory.Exists(path))
            {
                /*/ Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(path);
                foreach (string fileName in fileEntries)
                    ProcessFile(fileName);//*/

                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(path);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    string dirName = subdirectory.Substring(Settings.Data.PATH.Length);
                    table.Rows.Add(dirName);
                }

                dataGridView1.DataSource = table;
            }
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (view == 0)
            {// now youre selecting a client
                DataTable table = new DataTable();
                table.Columns.Add("Client Name", typeof(string));
                table.Columns.Add("Date", typeof(string));

                string[] selectedDirectories = Directory.GetDirectories(Settings.Data.PATH);
                string path = selectedDirectories[e.RowIndex];
                clientID = e.RowIndex;
                string selectedName = path.Substring(Settings.Data.PATH.Length);
                string[] subdirectoryEntries = Directory.GetDirectories(path);
                
                foreach (string subdirectory in subdirectoryEntries)
                {
                    string dirName = subdirectory.Substring(Settings.Data.PATH.Length + selectedName.Length);
                    table.Rows.Add(path.Substring(Settings.Data.PATH.Length), dirName);
                }
                view = 1;
                dataGridView1.DataSource = table;
                return;
            }
            if (view == 1)
            {// now youre selecting a directory with files
                string[] selectedDirectories = Directory.GetDirectories(Settings.Data.PATH);
                string[] selectedData = Directory.GetDirectories(selectedDirectories[clientID]);
                string[] fileEntries = Directory.GetFiles(selectedData[e.RowIndex]);

                foreach (string fileName in fileEntries)
                {
                    ReadFile(fileName);
                }

                this.Hide();

                data.Sort(
                    delegate(List<string> x, List<string> y)
                    {
                        if (Int32.Parse(x[Settings.Data.INDEX]) > Int32.Parse(y[Settings.Data.INDEX]))
                        {
                            return 1;
                        }
                        return -1;
                    });

                Graphs gui = new Graphs(data);
                gui.Show();
            }
            
            // je kan een gui aanroepen
        }

        private void ReadFile(string file)
        {
            List<string> ReceivedList = new List<string>();
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(List<string>);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        ReceivedList = (List<string>)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            data.Add(ReceivedList);
        }

        // connection stuff
        private void ConnectToServer()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6556), ConnectCallBack, null);
        }

        private void ConnectCallBack(IAsyncResult result)
        {
            if (socket.Connected)
            {
                Console.WriteLine("Connected!");
                buffer = new byte[1024];
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallBack, null);
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
            int bufLength = socket.EndReceive(result);
            byte[] packet = new byte[bufLength];
            Array.Copy(buffer, packet, packet.Length);

            // use packet..
            PacketHandler.Handle(packet, clientSocket);

            // do it again
            buffer = new byte[1024];
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallBack, null);
        }

        public void sendData(byte[] data)
        {
            try
            {
                socket.Send(data);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}
