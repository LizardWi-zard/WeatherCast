using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WeatherCast
{
    public class TimerEventManager
    {
        private Timer timer;
        private string path;
        FileInfo fileInf;
        DateTime dateTime;

        public TimerEventManager(string path)
        {
            
        }

        private void OnTimedEvent(Object sourse, System.Timers.ElapsedEventArgs e)
        {
            if (!fileInf.Exists)
            {
                fileInf.Create().Close();
                dateTime = DateTime.Now;
                using (StreamWriter sw = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, dateTime);
                }
            }
            else
            {
                dateTime = DateTime.Now;
                List<string> arrLine = new List<string>();
                arrLine = File.ReadAllLines(path).ToList();
                arrLine[1] = dateTime.ToString();
                File.WriteAllLines(path, arrLine);


            }
        }

    }
}
