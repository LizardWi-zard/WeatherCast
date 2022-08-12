using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.Helpers
{
    public class SettingsProvider
    {
        private static SettingsProvider instance;

        private SettingsProvider()
        { }

        public static SettingsProvider getInstance()
        {
            if (instance == null)
                instance = new SettingsProvider();
            return instance;
        }

        public List<string> CitiyNames { get; set; } = new List<string>();

        public DateTime LastRequestTime { get; set; }

        public void AddCity(string name)
        {
            if (CitiyNames.Count() <= 5)
            {
                CitiyNames.Add(name);
            }
            else
            {
                CitiyNames.RemoveAt(0);
                CitiyNames.Add(name);
            }
        }
    }
}
