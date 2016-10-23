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
        static Sim sim;
        static BikeConnection bike;

        static void Main(string[] args)
        {
            System.Timers.Timer timerT = new System.Timers.Timer(1000);
            String answer = "";
            while (answer != "sim" && answer != "bike")
            {
                Console.WriteLine("write sim for simulation - write bike for bikeConnection");
                answer = Console.ReadLine();
                Console.WriteLine(answer);
            }

            if (answer == "sim")
            {
                sim = new Sim();
                timerT.Elapsed += OnTimedEventSim;
            }
            else
            {
                bike = new BikeConnection();
                timerT.Elapsed += OnTimedEventBike;
            }

            timerT.Start();
            Console.ReadKey();

        }


        //timer action
        private static void OnTimedEventSim(Object source, ElapsedEventArgs e)
        {

            sim.SendCommand("st");

        }

        //timer action bike
        private static void OnTimedEventBike(Object source, ElapsedEventArgs e)
        {

            bike.SendCommand("st");

        }
    }
}