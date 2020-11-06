using DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataAccess.Data
{
    public class SettingsContext
    {
        private static Settings _settings { get; set; }

        public static IEnumerable<string> GetStatus()
        {
            var path = Directory.GetCurrentDirectory() + @"\jsonsettings.json";
            var json = File.ReadAllText(path);


            _settings = JsonConvert.DeserializeObject<Settings>(json);

            var list = new List<string>();

            foreach (var status in _settings.Status)
            {
                list.Add(status);
            }

            return list;
        }

        public static int GetMaxItemsCount()
        {
            var path = Directory.GetCurrentDirectory() + @"\jsonsettings.json";
            var json = File.ReadAllText(path);


            _settings = JsonConvert.DeserializeObject<Settings>(json);
            return _settings.MaxItemsCount;
        }

        public static IEnumerable<int> GetItemsCount()
        {
            var path = Directory.GetCurrentDirectory() + @"\jsonsettings.json";
            var json = File.ReadAllText(path);


            _settings = JsonConvert.DeserializeObject<Settings>(json);

            var list = new List<int>();

            foreach (var n in _settings.ItemsCount)
            {
                list.Add(n);
            }

            return list;
        }


    }

    public class Settings
    {
        public string[] Status { get; set; }
        public int MaxItemsCount { get; set; }
        public int [] ItemsCount { get; set; }
    }
}
