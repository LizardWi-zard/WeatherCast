using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WeatherCast
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        Timer timer = new Timer();
        string homeCity = "Берлин";
        DateTime lastRequestTime;
        
        protected CurrentWeather updatedInfo;
        protected WeatherService control;

        public ViewModelBase(WeatherService control, CurrentWeather weather)
        {
            updatedInfo = weather;
            this.control = control;
            timer.Interval = 1000 * 60 * 1;
            timer.AutoReset = true;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void OnTimedEvent(Object sourse, System.Timers.ElapsedEventArgs e)
        {
            //string path = @"D:\WeatherCast\requestTime.txt"; //TODO: Сдеать обработку исключений для некоректных данных
            FileInfo fileInf = new FileInfo(@"D:\WeatherCast\requestTime.txt");
            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                string line;
                string[] Lines = File.ReadAllLines(@"D:\WeatherCast\requestTime.txt");
                arrLine = Lines.ToList();
                
                //homeCity = arrLine[0];
                
                

                //foreach (string line in System.IO.File.ReadLines(path))
                //{
                //    arrLine.Add(line);
                //}

                //homeCity = arrLine[0];
                //lastRequestTime = DateTime.Parse(arrLine[1]);

                //if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                //{
                //    arrLine[1] = DateTime.Now.ToString();
                //}

                using (StreamReader reader = new StreamReader(@"D:\WeatherCast\requestTime.txt"))
                {
                    line = reader.ReadToEnd();
                    reader.Close();
                }

                arrLine = line.Split(" ").ToList();

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    arrLine[1] = DateTime.Now.ToString();
                    lastRequestTime = DateTime.Parse(arrLine[1]);
                }


                File.WriteAllLines(@"D:\WeatherCast\requestTime.txt", arrLine);
            }
            else
            {
                arrLine.Add(homeCity);
                arrLine.Add(DateTime.Now.ToString());
               
                lastRequestTime = DateTime.Now;

                File.Create(@"D:\WeatherCast\requestTime.txt").Close();
                File.WriteAllLines(@"D:\WeatherCast\requestTime.txt", arrLine);
            }

            string path = @"D:\WeatherCast\SelectedCityInfo.txt";
            fileInf = new FileInfo(path);

            if (!fileInf.Exists)
            {
                File.Create(path).Close();

                updatedInfo = control.GetCurrentWeather(homeCity);

                using (StreamWriter sw = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, updatedInfo);
                }
            }
            else
            {
                string response;

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 30)
                {
                    updatedInfo = control.GetCurrentWeather(homeCity);

                    using (StreamWriter sw = File.CreateText(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, updatedInfo);
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(path))
                    {
                        response = streamReader.ReadToEnd();
                    }

                    updatedInfo = JsonConvert.DeserializeObject<CurrentWeather>(response);

                    if (updatedInfo.Name.ToLower() != homeCity.ToLower())
                    {
                        updatedInfo = control.GetCurrentWeather(homeCity);
                    }
                }
            }
        }
    }
}
