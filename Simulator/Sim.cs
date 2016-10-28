using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace BikeAppA3
{
    public class Sim : Bike
    {
        public static Random rnd = new Random();
        public int Power { get; set; }
        public double Pulse { get; set; }
        public double RPM { get; set; }
        public double Speed { get; set; }
        public double Distance { get; set; }
        public SerialPort Port { get; set; }
        public bool Connected { get; set; }

        /**
        *** Constructor - initialises the Simulator
        **/
        public Sim(double pulse, double rpm, double speed, double distance)
        {
            Port = new SerialPort(); // no port is used with the simulation
            Pulse = pulse;
            RPM = rpm;
            Speed = speed;
            Distance = distance;
        }

        public Sim()
        {
            Port = new SerialPort(); // no port is used with the simulation
            Pulse = 1;
            RPM = 1;
            Speed = 1;
            Distance = 1;
            GenerateData();
        }
        
        /**
        *** @return List containing all the data necessary for display.
        **/
        public List<string> ReadData()
        {
            GenerateData();
            var data = new List<string>();
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

        /**
        *** Send a command to the machine, returning values or setting values.
        *** Most commands in the simulation will return parameters, because it has no physical machine.
        *** @command string - Input given by the user to send to the machine.
        **/
        public void SendCommand(string command)
        {
            //@TODO most commands
            switch (command.Substring(0, Math.Min(2, command.Length)))
            {
                case "CD":
                    break;
                case "CM":
                    break;
                case "ID":
                    Console.WriteLine("NOT SET");
                    break;
                case "PW":
                    ChangePower(command);
                    break;
                case "RD":
                    break;
                case "ST":
                    ReadData();
                    break;
                case "VE":
                    break;
                case "VS":
                    Console.WriteLine("Expected value; FORMAT VS <Value>");
                    break;
                case "VS%":
                    break;
                default:
                    Console.WriteLine("Unrecognised Command!");
                    break;
            }
        }

        public void ChangePower(string PW)
        {
            var power = PW.Substring(2, PW.Length - 2);
            var newPower = int.Parse(power.Trim());
            this.Power = newPower;
        }
        
        /**
        *** GenerateData generates the data to fill into the parameters.
        *** This is needed because the Simulation does not get any input values from a machine.
        **/
        public void GenerateData()
        {
            Pulse = rndInt(0, 120);
            RPM = rndInt(0, 110);
            Speed = rndDouble(0, 60);
            Distance = rndDouble(0, 999);
        }

        /**
        *** @return string containing all parameters(data)
        **/
        public override string ToString()
        {
            return string.Format("Pulse: {0} - RPM: {1} - Speed: {2} - Distance: {3}",
                Pulse,
                RPM,
                Speed,
                Distance);
        }

        public static double rndDouble(double min, double max)
        {
            var range = max - min;
            var difference = 0 + min;
            double newDouble;
            newDouble = rnd.NextDouble()*range + difference;
            return newDouble;
        }

        public static int rndInt(int min, int max)
        {
            int newInt;
            newInt = rnd.Next(min, max);
            return newInt;
        }
    }
}