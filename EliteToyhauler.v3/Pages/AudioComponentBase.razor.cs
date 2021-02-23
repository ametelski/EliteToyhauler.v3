using EliteToyhauler.v3.Application;
using EliteToyhauler.v3.Application.Audio;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EliteToyhauler.v3.Pages
{
    public class AudioComponetBase : ComponentBase
    {
        [Inject] 
        IAudioSettingsService service { get; set; }
        [Inject] 
        IDmp64Service AudioService { get; set; }

        [Parameter]
        public int zoneId { get; set; }
        public int _vol { get; set; }
        public bool _isMuted { get; set; } = true;
        public string _color = "blue";

        public int _max = 2108;
        public int _min = 1400;

        public int Volume
        {
            get => _vol;
            set
            {
                InvokeAsync(() => AudioService.SetVolume(zoneId, value));
                _vol = value;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var setting = service.GetAudioSettings(zoneId);
            _min = setting.Min;
            _max = setting.Max;
            AudioService.DataReceived += AudioChangeEvent;
            await UpdateState();
        }

        public async Task<int> SetVolumeAsync(ChangeEventArgs e)
        {
            var vol = int.Parse(e.Value.ToString());
            await AudioService.SetVolume(zoneId, vol);
            _vol = vol;
            return _vol;
        }

        public async Task SetMute()
        {
            _isMuted = _isMuted ? false : true;
            await AudioService.SetMute(zoneId, _isMuted);
            ChooseColor();
        }

        public async Task UpdateState()
        {
            _vol = await AudioService.GetVolume(zoneId);
            _isMuted = await AudioService.GetMute(zoneId);
            if (_isMuted)
            {
                _color = "red";
            }
            else
            {
                _color = "blue";
            }
            await InvokeAsync(StateHasChanged);
        }

        public void GetVolumeAfterRender()
        {
            InvokeAsync(StateHasChanged);
        }

        public void GetMuteAfterRender()
        {
            ChooseColor();
        }

        private void ChooseColor()
        {
            if (_isMuted)
            {
                _color = "red";
            }
            else
            {
                _color = "blue";
            }
            InvokeAsync(StateHasChanged);
        }

        private void AudioChangeEvent(object sender, AudioChangeEvent e)
        {

            UpdateState();
            InvokeAsync(StateHasChanged);
        }

    }
}
