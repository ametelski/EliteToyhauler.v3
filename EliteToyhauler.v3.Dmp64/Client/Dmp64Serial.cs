using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Microsoft.Extensions.Logging;

namespace EliteToyhauler.v3.Dmp64.Client
{
    public class Dmp64Serial : IDmp64TcpClient, IDisposable
    {
        private readonly SerialPort _serialPort;
        private readonly ILogger<Dmp64Serial> logger;

        public Dmp64Serial(ILogger<Dmp64Serial> logger)
        {
            _serialPort = new SerialPort { 
                PortName = "/dev/ttyAMA0",
                BaudRate = 38400,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One            
            };
            this.logger = logger;
        }

        public async Task<string> SendAsync(string message)
        {
            try
            {
                logger.LogInformation("Opening serial connection."); 
                _serialPort.Open();
                logger.LogInformation("Connection opened.");
                logger.LogInformation("Sending message ({message})", message); 
                _serialPort.WriteLine(message);

                var response = _serialPort.ReadLine();
                logger.LogInformation("Reading Line. ({response})", response); 
                _serialPort.Close();

                return response; 
            }
            catch (Exception ex)
            {
                logger.LogCritical("Failer: {@ex}", ex); 
                throw;
            }
        }

        public void Dispose()
        {
            _serialPort.Close();
            _serialPort.Dispose(); 
        }
    }
}
