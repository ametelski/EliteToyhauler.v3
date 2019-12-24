using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using EliteToyhauler.v3.Application; 

namespace EliteToyhauler.v3.Data
{
    public class DataStore : IDataStore
    {
        private Dictionary<int, int?> AudioValues = new Dictionary<int, int?>();
        private Dictionary<int, bool?> MuteValues = new Dictionary<int, bool?>();

        private static Mutex mut = new Mutex();

        public void RegisterZones(int zoneId)
        {
            if (AudioValues.ContainsKey(zoneId))
                return; 
            lock (AudioValues)
            {
                AudioValues.Add(zoneId, null);
                MuteValues.Add(zoneId, null); 
            }
        }

        public int? GetZoneVolume(int zoneId)
        {
            if (!AudioValues.ContainsKey(zoneId))
                RegisterZones(zoneId);

            lock (AudioValues)
                return AudioValues[zoneId];  
        }

        public bool? GetZoneMute(int zoneId)
        {
            if (!MuteValues.ContainsKey(zoneId))
                RegisterZones(zoneId);

            lock (MuteValues)
                return MuteValues[zoneId];
        }

        public void SetZoneVolume(int zoneId, int value)
        {
            if (!AudioValues.ContainsKey(zoneId))
                RegisterZones(zoneId);

            lock (AudioValues)
                AudioValues[zoneId] = value;
        }

        public void SetZoneMute(int zoneId, bool value)
        {
            if (!MuteValues.ContainsKey(zoneId))
                RegisterZones(zoneId);
            lock (MuteValues)
                MuteValues[zoneId] = value; 
        }
    }
}
