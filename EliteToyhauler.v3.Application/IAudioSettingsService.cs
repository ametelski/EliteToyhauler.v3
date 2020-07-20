using EliteToyhauler.v3.Application.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace EliteToyhauler.v3.Application
{
    public interface IAudioSettingsService
    {
        List<AudioSliderSettings> GetAudioSettings();
        AudioSliderSettings GetAudioSettings(int zone);
        void SaveSettings(List<AudioSliderSettings> settings);
        void SetMin(int zone, int min);
        void SetMax(int zone, int max); 
    }
}
