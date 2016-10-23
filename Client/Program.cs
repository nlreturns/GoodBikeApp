using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        private static Client Client;

        static void Main(string[] args)
        {
            Client = new Client();

            string msg = "Hello!";
            Message packet = new Message(msg);
            Client.conn.sendData(packet.Data);

            while (true)
            {
                List<string> data = Client.bike.ReadData();
                //Client.conn.sendData(data);
                //sendToVR(Client.bike.Speed);
            }
        }
    }
}
