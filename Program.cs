using System;
using SharpPcap;

namespace spcap2
{
    class Program
    {
        public static string eingabe;
        private static int devindex;

        static void Main(string[] args)
        {
            int r = 0;

            Console.Write("Geben Sie etwas ein: ");

            eingabe = Console.ReadLine();

            var devices = CaptureDeviceList.Instance;

            foreach (ICaptureDevice dev in devices)
            {
                Console.WriteLine(devices[r]);
                Console.WriteLine("-----------------------------------------------------------");
                Console.Write("\r\n");
              

                string namecheck = devices[r].ToString();

                

                if ( namecheck.Contains("FriendlyName: Ether"))

                        {
                     devindex = r;
                }

                r++;
            }

            Console.WriteLine("Ethernet string found in Index " +  devindex);
            Console.WriteLine();
            Console.WriteLine("Scanning ...");
            Console.WriteLine();



            try
            {
                ICaptureDevice device = devices[devindex];
                device.OnPacketArrival += new PacketArrivalEventHandler(E);
                int readTimeoutMilliseconds = 1;
                device.Open(DeviceModes.None, readTimeoutMilliseconds);

                //string filter = "ip and tcp";
                //string filter = "port 53";

                //device.Filter = filter;

                device.StartCapture();

                //device.Capture();

                Console.ReadLine();

                device.StopCapture();
                device.Close();
            }
            catch (Exception e)
            {
                Console.Write(e);
                Console.ReadKey();
            }
        }

        public static void E(object sender, PacketCapture e)
        {
            string efr = ""; // Deklaration und Initialisierung der Variable 'efr'

            for (int i = 0; i < e.Data.Length; i++)
            {
                var cc = ((char)e.Data[i]);

                if (cc >= 32 && cc <= 126)
                {
                    efr += cc;
                }
            }

            if (!string.IsNullOrEmpty(eingabe))
            {
                if (efr.IndexOf(eingabe, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine(DateTime.Now); // Ausgabe von Uhrzeit und Datum
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write(efr);
                    Console.Write("\r\n");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("\r\n");
                }
            }
        }

        private class CaptureEventArgs
        {
        }
    }
}
