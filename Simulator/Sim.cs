using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management.Instrumentation;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BikeAppA3
{
  public class Sim : Bike
  {
        
    public double Pulse { get; set; }
    public double RPM { get; set; }
    public double Speed { get; set; }
    public double Distance { get; set; }
    public SerialPort Port { get; set; }
    public static Random rnd = new Random();
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
        System.Collections.Generic.List<string> data = new List<string>();
        try
        {
            data.Add(Pulse.ToString());
            data.Add(Port.ToString());
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
      switch (command)
      {
        case "CD":
          break;
        case "CM":
          break;
        case "ID":
          Console.WriteLine("NOT SET");
              break;
        case "PW":
          Console.WriteLine("Expected value; FORMAT PW <Value>");
              break;
        case "PW%":
          break;
        case "RD":
          break;
        case "ST":
          ReadData();
          break;
        case "VE":
          break;
        case "VS":
          Console.WriteLine("Expected value; FORMA VS <Value>");
              break;
        case "VS%":
          break;
        default:
          Console.WriteLine("Unrecognised Command!");
          break;
      }
    }

    /**
    *** Saves the data to the database
    *** @TODO figure out table fields, query, database class usage etc.
    **/

    public string SaveToDatabase(string query)
    {
      throw new NotImplementedException();
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
        return String.Format("Pulse: {0} - RPM: {1} - Speed: {2} - Distance: {3}",
            this.Pulse.ToString(),
            this.RPM.ToString(),
            this.Speed.ToString(),
            this.Distance.ToString());
    }

    public static double rndDouble(double min, double max)
    {
      double range = max - min;
      double difference = 0 + min;
      double newDouble;
      newDouble = (rnd.NextDouble() * range) + difference;
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