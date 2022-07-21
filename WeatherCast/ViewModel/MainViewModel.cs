using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using WeatherCast.Core;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private CurrentWeather currentWeather;
        private object _currentView;
        private DateTime lastRequestTime;
        private string selectedCity = "Москва";
        private Timer timer = new Timer();
        private WeatherService control;

        public MainViewModel()
        {
            control = new WeatherService();
            
            VMBase = new ViewModelBase(control);

            currentWeather = SaveData(control);

            HomeVM = new HomeViewModel(control, currentWeather);
            SettingsVM = new SettingsViewModel(Definitions.DefaultCity);

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });

            /*
            SearchVM = new SearchViewModel();

            SearchCommand = new RelayCommand(o =>
            {
                Response = currentWeather;
                SearchVM.UpdateControlResponse(control, Response);

                CurrentView = SearchVM;
            });
            
            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = SearchVM;
            });
            */

            timer.Interval = 1000 * 60 * 1;
            timer.AutoReset = true;
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        public ViewModelBase VMBase { get; set; }

        public HomeViewModel HomeVM { get; set; }

        public SearchViewModel SearchVM { get; set; }

        public SettingsViewModel SettingsVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand SearchViewCommand { get; set; }

        public RelayCommand SettingsViewCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public CurrentWeather Response { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private void OnTimedEvent(Object sourse, System.Timers.ElapsedEventArgs e)
        {
            Response = SaveData(control);

            HomeVM.CurrentWeather = Response;
        }

        public CurrentWeather SaveData(WeatherService control)
        {
           
            FileInfo fileInf = new FileInfo(Definitions.RequestTimePath);

            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(Definitions.RequestTimePath).ToList();
                selectedCity = arrLine[0];
                lastRequestTime = DateTime.Parse(arrLine[1]);

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    arrLine[1] = DateTime.Now.ToString();
                }

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }
            else
            {
                arrLine.Add(Definitions.DefaultCity);
                arrLine.Add(DateTime.Now.ToString());

                selectedCity = Definitions.DefaultCity;
                lastRequestTime = DateTime.Now;

                File.Create(Definitions.RequestTimePath).Close();

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }

            fileInf = new FileInfo(Definitions.SelectedCityCurrenWeatherInfoPath);



            if (!fileInf.Exists)
            {
                File.Create(Definitions.SelectedCityCurrenWeatherInfoPath).Close();

                currentWeather = control.GetCurrentWeather(selectedCity);

                using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, currentWeather);
                    sw.Close();
                }

                return currentWeather;
            }
            else
            {
                string response;

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    currentWeather = control.GetCurrentWeather(selectedCity);

                    using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, currentWeather);
                        sw.Close();
                    }

                    return currentWeather;
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityCurrenWeatherInfoPath))
                    {
                        response = streamReader.ReadToEnd();
                        streamReader.Close();
                    }

                    currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(response);

                    if (currentWeather.Name.ToLower() != selectedCity.ToLower())
                    {
                        currentWeather = control.GetCurrentWeather(selectedCity);

                        using (StreamWriter sw = File.CreateText(Definitions.SelectedCityCurrenWeatherInfoPath))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(sw, currentWeather);
                            sw.Close();
                        }
                    }

                    return currentWeather;
                }
            }
        }
    }
}
