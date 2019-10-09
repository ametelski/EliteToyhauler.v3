using System;
using System.Collections.Generic;
using System.Text;

namespace EliteToyhauler.v3.Application
{
    public interface IDataStore
    {
        int GetZoneVolume(int zoneId);
        bool GetZoneMute(int zoneId);
        void SetZoneVolume(int zoneId, int value);
        void SetZoneMute(int zoneId, bool value); 
    }
}
