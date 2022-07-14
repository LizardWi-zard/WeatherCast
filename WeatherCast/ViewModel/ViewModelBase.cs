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

        public ViewModelBase(WeatherService control)
        {
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
            updatedInfo = SaveData(control);
        }

        public CurrentWeather SaveData(WeatherService control)
        {
            //TODO: Сдеать обработку исключений для некоректных данных
            FileInfo fileInf = new FileInfo(Definitions.RequestTimePath);

            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(Definitions.RequestTimePath).ToList();
                homeCity = arrLine[0];
                lastRequestTime = DateTime.Parse(arrLine[1]);
                arrLine[0] = "Ярославль";

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    arrLine[1] = DateTime.Now.ToString();
                }

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }
            else
            {
                arrLine.Add("Москва");
                arrLine.Add(DateTime.Now.ToString());

                homeCity = "Москва";
                lastRequestTime = DateTime.Now;

                // TODO добавить проверку на существование папки, при первом запуске её нет и она в этом случае никем не создаётся

                File.Create(Definitions.RequestTimePath).Close();

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }

            fileInf = new FileInfo(Definitions.SelectedCityCurrenWeatherInfoPath);

            if (!fileInf.Exists)
            {
                File.Create(Definitions.SelectedCityCurrenWeatherInfoPath).Close();

                updatedInfo = control.GetCurrentWeather(homeCity);

                using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, updatedInfo);
                    sw.Close();
                }

                return updatedInfo;
            }
            else
            {
                string response;

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    updatedInfo = control.GetCurrentWeather(homeCity);

                    using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, updatedInfo);
                        sw.Close();
                    }

                    return updatedInfo;
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityCurrenWeatherInfoPath))
                    {
                        response = streamReader.ReadToEnd();
                        streamReader.Close();
                    }

                    updatedInfo = JsonConvert.DeserializeObject<CurrentWeather>(response);

                    if (updatedInfo.Name.ToLower() != homeCity.ToLower())
                    {
                        updatedInfo = control.GetCurrentWeather(homeCity);
                    }

                    return updatedInfo;
                }
            }
        }
    }
}
