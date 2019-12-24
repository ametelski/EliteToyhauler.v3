using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteToyhauler.v3.Application.Audio
{
    public interface IDmp64Service
    {
        Task<bool> GetMute(int zoneId);
        Task <int> GetVolume(int zoneId);
        Task SetMute(int zoneId, bool isMuted);
        Task SetVolume(int zoneId, int volume);
        event EventHandler<AudioChangeEvent> DataReceived;
    }
}
