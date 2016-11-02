using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net.Mime;
using System.Threading;

namespace BikeAppA3
{
    public class BikeConnection : Bike
    {
        public SerialPort sp;
        public List<String[]> data = new List<String[]>();
        public bool Connected { get; set; }
        public double Pulse { get; set; }
        public double RPM { get; set; }
        public double Speed { get; set; }
        public double Distance { get; set; }
        public SerialPort Port { get; set; }

        public BikeConnection()
        {
            string[] pn = SerialPort.GetPortNames();
            Console.WriteLine(pn.Length);
            if (pn.Length == 0 || pn.Length == 1)
            {
                Console.WriteLine("there is no COM port available, booting Simulation");
                Connected = false;
            }
            else
            {
                sp = new SerialPort(pn[0], 9600);
                sp.Open();
                // get ID as test if connection excists.
                SendCommand("ID");
                Thread.Sleep(200);
                string bikeID = Listen();
                Console.WriteLine(bikeID);
                
                // reset the bike
                SendCommand("rs");
                Console.WriteLine(sp.ReadLine());
                Thread.Sleep(500);
                // put bike in command state
                SendCommand("CM");
                Console.WriteLine(sp.ReadLine());

                Connected = true;
            }
        }

        public string Listen()
        {
            string s = "";
            try
            {
                s = sp.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("USB cable not plugged in / PC-bike simulation error");
            }
            return s;
        }

        public void SendCommand(string s)
        {
            try
            {
                sp.WriteLine(s);
            }
            catch
            {
                Console.WriteLine("USB cable not plugged in / PC-bike simulation error");
            }
        }
        
        public List<string> ReadData()
        {
            SendCommand("ST");
            var allData = Listen();
            var letters = allData.Split("\t".ToCharArray());
            Pulse = double.Parse(letters[Settings.Data.BPM]);
            RPM = double.Parse(letters[Settings.Data.RPM]);
            Speed = double.Parse(letters[Settings.Data.SPEED]);
            Distance = double.Parse(letters[Settings.Data.DISTANCE]);

            List<string> data = new List<string>();
            try
            {
                data.Add(Pulse.ToString());
                data.Add(RPM.ToString());
                data.Add(Speed.ToString());
                data.Add(Distance.ToString());
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }

            return data;
        }
        
    }
}