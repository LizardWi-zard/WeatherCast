using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private Timer timer = new Timer();
        private string homeCity = "Берлин";
        private DateTime lastRequestTime;
        protected CurrentWeather updatedInfo;
        protected WeatherService control;
        private string currentDirectory = Directory.GetCurrentDirectory();

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
            string pathToSave = Path.Combine(currentDirectory, "requestTime.txt");
            FileInfo fileInf = new FileInfo(pathToSave);

            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(pathToSave).ToList();
                homeCity = arrLine[0];
                lastRequestTime = DateTime.Parse(arrLine[1]);
                arrLine[0] = "Ярославль";

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    arrLine[1] = DateTime.Now.ToString();
                }

                File.WriteAllLines(pathToSave, arrLine);
            }
            else
            {
                arrLine.Add("Москва");
                arrLine.Add(DateTime.Now.ToString());

                homeCity = "Москва";
                lastRequestTime = DateTime.Now;

                File.Create(pathToSave).Close(); ;

                File.WriteAllLines(pathToSave, arrLine);
            }

            pathToSave = Path.Combine(currentDirectory, "SelectedCityInfo.txt");

            fileInf = new FileInfo(pathToSave);

            if (!fileInf.Exists)
            {
                File.Create(pathToSave).Close();

                updatedInfo = control.GetCurrentWeather(homeCity);

                using (StreamWriter sw = File.CreateText(pathToSave))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, updatedInfo);
                    sw.Close();
                }
            }
            else
            {
                string response;

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 30)
                {
                    updatedInfo = control.GetCurrentWeather(homeCity);

                    using (StreamWriter sw = File.CreateText(pathToSave))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, updatedInfo);
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(pathToSave))
                    {
                        response = streamReader.ReadToEnd();
                        streamReader.Close();
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
