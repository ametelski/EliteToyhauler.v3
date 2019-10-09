using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using EliteToyhauler.v3.Application; 

namespace EliteToyhauler.v3.Data
{
    public class DataStore : IDataStore
    {
        private string[] data = { "1501", "1501", "1500", "1500", "1500", "1", "1", "1", "1", "1" };

        private Dictionary<int, int> AudioValues = new Dictionary<int, int>();
        private Dictionary<int, bool> MuteValues = new Dictionary<int, bool>();

        private static Mutex mut = new Mutex();

        public void RegisterZones(int zoneId)
        {
            if (AudioValues.ContainsKey(zoneId))
                return; 
            lock (AudioValues)
            {
                AudioValues.Add(zoneId, 1501);
                MuteValues.Add(zoneId, true); 
            }
        }

        public int GetZoneVolume(int zoneId)
        {
            if (!AudioValues.ContainsKey(zoneId))
                RegisterZones(zoneId);

            lock (AudioValues)
                return AudioValues[zoneId];  
        }

        public bool GetZoneMute(int zoneId)
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


        //public string GetDataByIndex(int index)
        //{
        //    if (index > data.Length)
        //        return "0";
        //    mut.WaitOne();
        //    var value = data[index];
        //    mut.ReleaseMutex();
        //    return value;
        //}

        //public void SetAllData(string[] newData)
        //{
        //    mut.WaitOne();
        //    data = newData;
        //    mut.ReleaseMutex();
        //}

        //public bool SetValue(int index, string value)
        //{ 
        //    mut.WaitOne();
        //    if (data[index] == value) return false; 
        //    data[index] = value;
        //    mut.ReleaseMutex();
        //    return true; 
        //}
    }
}
