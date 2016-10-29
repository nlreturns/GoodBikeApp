using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Settings
{
    public class Data
    {
        public static readonly string IPSERVER = "";
        public static readonly string PORTSERVER = "";

        // data numbers
        public static readonly int BPM = 0;
        public static readonly int RPM = 1;
        public static readonly int SPEED = 2;
        public static readonly int DISTANCE = 3;
        public static readonly int INDEX = 4;
        public static readonly int REQPOWER = 4;
        public static readonly int CALORIES = 5;
        public static readonly int DURATION = 6;
        public static readonly int POWER = 7;

        // bike powers
        public static readonly int WARMUP = 75;
        public static readonly int NORMAL = 100;
        public static readonly int WATTADDED = 25;
        public static readonly int COOLINGDOWN = 75;

        // interval seconds
        public static readonly int INTERVAL = 1000;
        public static readonly int CALLSAT1MIN = 60000/INTERVAL;
        public static readonly int CALLSAT2MIN = 120000/INTERVAL;
        public static readonly int CALLSAT3MIN = 180000/INTERVAL;
        public static readonly int CALLSAT4MIN = 240000/INTERVAL;
        public static readonly int CALLSAT6MIN = 360000/INTERVAL;
        public static readonly int CALLSAT15SEC = 15000/INTERVAL;
        public static readonly int CALLSAT30SEC = 30000/INTERVAL;


    }
}
