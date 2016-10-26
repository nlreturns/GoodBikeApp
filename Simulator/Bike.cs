using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Security.Cryptography.X509Certificates;

namespace BikeAppA3
{
  public interface Bike
  {
    double Pulse { get; set; }
    double RPM { get; set; }
    double Speed { get; set; }
    double Distance { get; set; }
    SerialPort Port { get; set; }
    bool Connected { get; set; }

    List<string> ReadData();
    void SendCommand(string command);

  }
}