using System;
using System.IO;
using System.Linq;

namespace EliteToyhauler.v3.Sensor
{
    public class Temperature
    {
        public double GetTemperature()
        {
            double tempInF = 1.0;
            try
            {
                DirectoryInfo devicesDir = new DirectoryInfo("/sys/bus/w1/devices");
                foreach (var deviceDir in devicesDir.EnumerateDirectories("28*"))
                {
                    var w1slavetext =
                        deviceDir.GetFiles("w1_slave").FirstOrDefault().OpenText().ReadToEnd();
                    string temptext =
                        w1slavetext.Split(new string[] { "t=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                    double tempInC = double.Parse(temptext) / 1000;
                    tempInF = (tempInC * 9 / 5) + 32;

                    Console.WriteLine(string.Format("Device {0} reported temperature {1}C",
                        deviceDir.Name, tempInF));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read file. " + ex.Message);
            }
            return tempInF;
        }
    }
}
