using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientGUI : Form
    {
        public Client client;
        public System.Timers.Timer timer = new System.Timers.Timer();
        public int totalCalls = 0;
        public List<int> heartBeats = new List<int>(10);
        public List<List<string>> allData = new List<List<string>>();
        public int times;

        public ClientGUI(Client client)
        {
            InitializeComponent();
            this.client = client;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = Settings.Data.INTERVAL;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "START")
            {
                // reset total calls and enable timer
                totalCalls = 0;
                timer.Enabled = true;
                button1.Text = "STOP";
                // connect to the bike
                client.ConnectBike();
                // instruct client and set power to starting power
                AppendToTextBox("Neem plaats op de fiets");
                client.bike.SendCommand("PW" + Settings.Data.WARMUP);
                AppendToTextBox("U krijgt nu een warming-up van 2 minuten. Hierna word de testbelasting ingesteld.");
                AppendToTextBox("Houd uw omwentelingen tussen de 50-60");
                //Thread.Sleep(120000)
            }
            else
            {
                timer.Enabled = false;
                button1.Text = "START";
            }
        }

        private void AppendToTextBox(string text)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                textBox.Text += Environment.NewLine + text;
            });
            this.Invoke(invoker);
        }

        private void EndTest()
        {
            // get the median of the heartbeat list
            List<int> HB = new List<int>(10);
            HB = heartBeats;
            HB.Sort();
            int median = HB[5];
            // calculate the average
            int average;
            if (median - HB[0] > 5 || HB[9] - median > 5)
            {
                //no constant heartbeat. Calculate the average of last 2 minutes
                int total = heartBeats.Skip(2).Take(8).Sum();
                average = total/8;
            }
            else
            { // constant heartbeat. Just take the median.
                average = median;
            }

            // determine factor.
            double factor = 1.00;
            // check maximum hb
            if (HB[9] >= 150)
            {
                factor = 1.12;
                if (HB[9] >= 160)
                {
                    factor = 1.00;
                    if (HB[9] >= 170)
                    {
                        factor = 0.93;
                        if (HB[9] >= 180)
                        {
                            factor = 0.83;
                            if (HB[9] >= 190)
                            {
                                factor = 0.75;
                                if (HB[9] >= 200)
                                {
                                    factor = 0.69;
                                    if (HB[9] >= 210)
                                    {
                                        factor = 0.64;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // check age
            if (client.age >= 35)
            {
                factor = 0.87;
                if (client.age >= 40)
                {
                    factor = 0.83;
                    if (client.age >= 45)
                    {
                        factor = 0.78;
                        if (client.age >= 50)
                        {
                            factor = 0.75;
                            if (client.age >= 55)
                            {
                                factor = 0.71;
                                if (client.age >= 60)
                                {
                                    factor = 0.68;
                                    if (client.age >= 65)
                                    {
                                        factor = 0.65;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // calculate zuurstofverbruik
            double VO2max = (((0.00212*(Settings.Data.NORMAL + (Settings.Data.WATTADDED*times))) + 0.299)/
                             (((0.769*average) - 48.5)*100));
            // apply factor
            VO2max *= factor;
            AppendToTextBox(VO2max.ToString() + " is uw resultaat.");
            // alles saven.
            Message namePacket = new Message(client.name);
            client.conn.sendData(namePacket.Data);
            foreach (List<string> l in allData)
            {
                Message packet = new Message(l);
                client.conn.sendData(packet.Data);
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // get the current data
            List<string> data = client.bike.ReadData();
            // save it in the total list
            allData.Add(data);
            // set the rounds per minute
            this.Invoke((MethodInvoker) delegate
            {
                RPM.Text = data[Settings.Data.RPM];
            });
            // add one to the total calls
            totalCalls++;
            
            // check if heartbeat doesnt reach maximum!!
            if (Int32.Parse(data[Settings.Data.BPM]) > client.limit)
            {
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
                AppendToTextBox("BEINDIG DE TEST METEEN, HART RITME TE HOOG");
            }
            

            // 2 minutes warming up completed, start the test
            if (totalCalls == Settings.Data.CALLSAT2MIN)
            {
                AppendToTextBox("Testbelasting word toegepast");
                client.bike.SendCommand("PW" + Settings.Data.NORMAL);
            }
            // per 3, 8x opnemen
            if (totalCalls >= Settings.Data.CALLSAT2MIN && totalCalls <= Settings.Data.CALLSAT4MIN)
            {
                if (totalCalls % Settings.Data.CALLSAT1MIN == 0)
                { // get the heartbeat every minute
                    if (Int32.Parse(data[Settings.Data.BPM]) < 130)
                    {// if the heartbeat is below 130 
                        // set the calls 30 seconds back and raise power
                        times++;
                        totalCalls -= (Settings.Data.CALLSAT30SEC);
                        client.bike.SendCommand("PW" + (Settings.Data.NORMAL + (Settings.Data.WATTADDED*times)));
                    }
                    else
                    {// add the heartbeat
                        heartBeats.Add(Int32.Parse(data[Settings.Data.BPM]));
                    }

                }
            }
            else if (totalCalls >= Settings.Data.CALLSAT4MIN)
            {
                // get the heartbeat every 15 seconds
                if (totalCalls % Settings.Data.CALLSAT15SEC == 0)
                {
                    heartBeats.Add(Int32.Parse(data[Settings.Data.BPM]));
                }
            }
            else
            {
                // still doing warming up
            }

            // stop making calls
            if (totalCalls % Settings.Data.CALLSAT6MIN == 0)
            {
                timer.Enabled = false;
                AppendToTextBox("De cooling-down van 2 minuten begint nu. Trap op een langzame snelheid door.");
                AppendToTextBox("De test verstuurd ondertussen data en rond af.");
                button1.Text = "COOLINGDOWN";
                EndTest();
            }

        }

    }
}
