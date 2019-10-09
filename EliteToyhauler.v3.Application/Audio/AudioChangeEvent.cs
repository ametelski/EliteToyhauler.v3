using System;
using System.Collections.Generic;
using System.Text;

namespace EliteToyhauler.v3.Application.Audio
{
    public class AudioChangeEvent : EventArgs
    {
        public int Zone { get; set; }
    }
}
