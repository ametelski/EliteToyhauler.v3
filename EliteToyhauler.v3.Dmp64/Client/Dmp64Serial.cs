using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace EliteToyhauler.v3.Dmp64.Client
{
    class Dmp64Serial : IDmp64TcpClient
    {



        public Task<string> SendAsync(string message)
        {
            var serialPort = new SerialPort();

            throw new NotImplementedException(); 
        }
    }
}
