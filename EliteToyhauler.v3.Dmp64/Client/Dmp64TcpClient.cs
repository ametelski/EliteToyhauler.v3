using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EliteToyhauler.v3.Dmp64.Client
{
    public class Dmp64TcpClient : IDmp64TcpClient
    {
        private readonly ILogger<Dmp64TcpClient> _logger;
        private readonly string _ipAddress;
        private TcpClient _client;
        int timeout = 5000;

        public Dmp64TcpClient(IOptionsMonitor<Dmp64Settings> settings, ILogger<Dmp64TcpClient> logger)
        {
            _logger = logger; 
            _ipAddress = settings.CurrentValue.IpAddress; 
        }

        public async Task Connect()
        {
            try
            {
                if (_client != null && _client.Connected && SocketIsConnected())
                    return;
                _logger.LogDebug("Creating a new TCP client.");
                _client = new TcpClient();
                await _client.ConnectAsync(_ipAddress, 23).ConfigureAwait(false);    

                await ReadStartUpMessage().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Failed to connect to Dmp 64."); 
            }
        }

        private async Task ReadStartUpMessage()
        {
            var netstream = _client.GetStream();
            //using var reader = new StreamReader(netstream);
            // Optionally set a timeout
            netstream.ReadTimeout = timeout;
            if (netstream.CanRead)
            {
                byte[] myReadBuffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;

                // Incoming message may be larger than the buffer size.
                do
                {
                    numberOfBytesRead = netstream.Read(myReadBuffer, 0, myReadBuffer.Length);

                    myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                }
                while (netstream.DataAvailable);

                // Print out the received message to the console.
                Console.WriteLine("You received the following message : " +
                                             myCompleteMessage);
            }
            else
            {
                Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
            }
        }

        private bool SocketIsConnected()
        {
            bool part1 = _client.Client.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_client.Available == 0);
            if (part1 && part2)
            {
                _logger.LogCritical("Closing connection."); 
                _client.Close();
                _client.Dispose();
                return false;
            }
            else
                return true;
        }

        public async Task<string> SendAsync(string message)
        {
            try
            {
                await Connect().ConfigureAwait(false);
                _logger.LogTrace($"Sending Message: {message}");
                return await WriteAndReadAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return null; 
            }
            
        }

        private async Task<string> WriteAndReadAsync(string message)
        {
            var netstream = _client.GetStream();

            if (netstream.CanWrite)
            {
                var bytes = Encoding.ASCII.GetBytes(message);
                await netstream.WriteAsync(bytes, 0, bytes.Length);
                if (netstream.CanRead)
                {
                    byte[] myReadBuffer = new byte[1024];
                    StringBuilder myCompleteMessage = new StringBuilder();
                    int numberOfBytesRead = 0;

                    // Incoming message may be larger than the buffer size.
                    do
                    {
                        numberOfBytesRead = netstream.Read(myReadBuffer, 0, myReadBuffer.Length);

                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                    }
                    while (netstream.DataAvailable);

                    _logger.LogTrace($"Received message: {myCompleteMessage}");
                    return myCompleteMessage.ToString(); 
                }
            }
            return null; 
        }
    }
}
