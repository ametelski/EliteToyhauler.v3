using EliteToyhauler.v3.Application;
using EliteToyhauler.v3.Application.Audio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace EliteToyhauler.v3.Data
{
    public class SettingsStore : IAudioSettingsService
    {
        private string _filePath;
        private List<AudioSliderSettings> _audioSettings { get; set; }

        public SettingsStore()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _filePath = Path.Combine(path, "audioSlidersSettings.json");
            var jsonFile = File.ReadAllText(_filePath);
            _audioSettings = JsonSerializer.Deserialize<List<AudioSliderSettings>>(jsonFile);
        }
        public void SaveSettings(List<AudioSliderSettings> settings)
        {
            _audioSettings = settings; 
            var jsonString = JsonSerializer.Serialize(_audioSettings);
            File.WriteAllText(_filePath, jsonString);
        }

        public void SetMin(int zone, int min)
        {
            _audioSettings.First(i => i.Zone == zone).Min = min; 
        }

        public void SetMax(int zone, int max)
        {
            _audioSettings.First(i => i.Zone == zone).Max = max;
        }

        public List<AudioSliderSettings> GetAudioSettings()
        {
            return _audioSettings; 
        }

        public AudioSliderSettings GetAudioSettings(int zone)
        {
            return _audioSettings.First(i => i.Zone == zone); 
        }
    }
}
