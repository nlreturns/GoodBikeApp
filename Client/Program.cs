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
            //*
            List<string> testList = new List<string>();
            testList.Add("Hoi");
            testList.Add("Hello");
            testList.Add("Oui");
            testList.Add("Swag");
            testList.Add("KK");
            Message packet = new Message(testList);
            Client.conn.sendData(packet.Data);
            /*/
            TestClass sendObj = new TestClass();
            Message packet = new Message((object)sendObj);
            Client.conn.sendData(packet.Data);//*/
            while (true)
            {
                //List<string> data = Client.bike.ReadData();
                //Client.conn.sendData(data);
                //sendToVR(Client.bike.Speed);
            }
        }
    }
}
