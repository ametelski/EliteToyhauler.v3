using EliteToyhauler.v3.Application;
using EliteToyhauler.v3.Application.Audio;
using EliteToyhauler.v3.Dmp64.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteToyhauler.v3.Dmp64.Service
{
    public class Dmp64Service : IDmp64Service
    {
        private readonly ILogger<Dmp64Service> _logger;
        private readonly IDmp64TcpClient _client;
        private readonly IDataStore _store;
        public event EventHandler<AudioChangeEvent> DataReceived;

        public Dmp64Service(ILogger<Dmp64Service> logger, IDmp64TcpClient client, IDataStore store)
        {
            _logger = logger;
            _client = client;
            _store = store; 
        }

        public async Task<bool> GetMute(int zoneId)
        {
            var isMuted = _store.GetZoneMute(zoneId);

            if (isMuted != null)
                return (bool)isMuted; 
           
            var result = await _client.SendAsync($"\x1BM{zoneId}AU\r").ConfigureAwait(false);
            isMuted = result == "1" ? true : false; 
            _store.SetZoneMute(zoneId, (bool)isMuted);
            return (bool)isMuted; 
        }

        public async Task<int> GetVolume(int zoneId)
        {
            var vol = _store.GetZoneVolume(zoneId);

            if (vol != null)
                return (int)vol; 

            var result = await _client.SendAsync($"\x1BG{zoneId}AU\r").ConfigureAwait(false);
            int value = int.Parse(result.TrimStart('0'));
            _store.SetZoneVolume(zoneId, value);
            return value; 
        }

        public async Task SetMute(int zoneId, bool isMuted)
        {
            int mute = isMuted ? 1 : 0;
            _logger.LogInformation($"Setting mute for zone {zoneId} to {mute}.");
            var value = await _client.SendAsync($"\x1BM{zoneId}*{mute}AU\r").ConfigureAwait(false);
            _store.SetZoneMute(zoneId, isMuted); 
            _logger.LogInformation($"Mute set for zone {zoneId} to {mute}. Invoking Event");

            DataReceived?.Invoke(this, new AudioChangeEvent { Zone = zoneId });
        }

        public async Task SetVolume(int zoneId, int volume)
        {
            _logger.LogInformation($"Setting volume for zone {zoneId}. ");
            var value = await _client.SendAsync($"\x1BG{zoneId}*{volume}AU\r").ConfigureAwait(false);
            _store.SetZoneVolume(zoneId, volume); 
            _logger.LogInformation($"Volume set for zone {zoneId}. \n\n");
            
            DataReceived?.Invoke(this, new AudioChangeEvent { Zone = zoneId}); 
        }
    }
}
