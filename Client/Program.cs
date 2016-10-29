using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        private static Client Client;

        static void Main(string[] args)
        {
            Client = new Client(15);
            Application.Run(new ClientGUI(Client));
            /*
            string name = "ClientName";
            Message namePacket = new Message(name);
            Client.conn.sendData(namePacket.Data);
            List<string> testList = new List<string>();
            testList.Add("Hoi");
            testList.Add("Hello");
            testList.Add("Oui");
            testList.Add("Swag");
            testList.Add("KK");
            testList.Add("0");
            Message packet = new Message(testList);
            Client.conn.sendData(packet.Data);
            List<string> secondList = new List<string>();
            secondList.Add("a");
            secondList.Add("b");
            secondList.Add("1");
            Message packet2 = new Message(secondList);
            Client.conn.sendData(packet2.Data);
            /*
            TestClass sendObj = new TestClass();
            Message packet = new Message((object)sendObj);
            Client.conn.sendData(packet.Data);//*/
            /*
            string test = "Clientnaam";
            Message packet = new Message(test);
            Client.conn.sendData(packet.Data);
            int index = 0;//*/
            /*while (true)
            {/*
                List<string> data = Client.bike.ReadData();
                data.Add(index.ToString());
                index++;
                Message packetData = new Message(data);
                Client.conn.sendData(packetData.Data);//*/
            //}
        }
    }
}
