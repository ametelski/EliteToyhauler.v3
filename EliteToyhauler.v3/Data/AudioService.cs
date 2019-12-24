using EliteToyhauler.v3.Application.Audio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EliteToyhauler.v3.Data
{
    public class AudioService : ComponentBase
    {
        public readonly ILogger<AudioService> _logger;
        private IDmp64Service _audioService;

        [Parameter]
        public int ZoneId { get; set; }
        public int Volume { get; set; }
        public bool IsMuted { get; set; }
        public string Color { get; set; }

        public AudioService(ILogger<AudioService> logger, IDmp64Service audioService)
        {
            _logger = logger; 
            _audioService = audioService;
            _audioService.DataReceived += AudioChangeEvent;
        }

        protected override void OnInitialized()
        {
            GetVolume();
            GetMute();
        }

        public void GetVolume()
        {
           //Volume = _audioService.GetVolume(ZoneId);
           InvokeAsync(() => { StateHasChanged(); });
        }

        public void GetMute()
        {
            //IsMuted = _audioService.GetMute(ZoneId);
            ChooseColor(); 
        }

        public async Task SetVolumeAsync(ChangeEventArgs e)
        {
            var vol = int.Parse(e.Value.ToString());
            await _audioService.SetVolume(ZoneId, vol).ConfigureAwait(false);
            Volume = vol; 
        }

        public async Task SetMute()
        {
            IsMuted = IsMuted ? false : true;
            await _audioService.SetMute(ZoneId, IsMuted).ConfigureAwait(false);
            ChooseColor();
        }

        private void ChooseColor()
        {
            if (IsMuted)
            {
                Color = "red";
            }
            else
            {
                Color = "blue";
            }
            InvokeAsync(() => { StateHasChanged(); });
        }

        private void AudioChangeEvent(object sender, AudioChangeEvent e)
        {
            _logger.LogWarning("Audio Change Event was received."); 
            GetMute();
            GetVolume();
        }
    }
}
