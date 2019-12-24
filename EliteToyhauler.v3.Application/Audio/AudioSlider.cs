using System;
using System.Collections.Generic;
using System.Text;

namespace EliteToyhauler.v3.Application.Audio
{
    public class AudioSlider
    {
        public int Zone { get; set; }
        
        public int Min { get; set; }
        
        public int Max { get; set; }

        public bool IsMuted { get; set; }
    }
}
