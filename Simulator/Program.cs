using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BikeAppA3
{
    class Program
    {
        static Bike bike;

        static void Main(string[] args)
        {
            Timer timerT = new Timer(1000);
            bike = new BikeConnection();

            if (bike.Connected)
            {

            }
            else
            {
                bike = new Sim();
            }

            timerT.Elapsed += OnTimedEventBike;
            timerT.Start();

            //change power
            //bike.sendCommand("PW" + deltaHoogte);
        }
        
        private static void OnTimedEventBike(Object source, ElapsedEventArgs e)
        {
            bike.SendCommand("st");
        }
    }
}