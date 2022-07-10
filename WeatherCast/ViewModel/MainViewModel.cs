using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherCast.Core;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private CurrentWeather currentWeather;
        private object _currentView;
        private WeatherService control;
        private DateTime lastRequestTime;
        private string homeCity = "Иваново";
        private string currentDirectory = Directory.GetCurrentDirectory();

        public MainViewModel()
        {
            WeatherService control = new WeatherService();

            currentDirectory = Directory.GetCurrentDirectory();
            
            SaveData(control);

            VMBase = new ViewModelBase(control, currentWeather);

            HomeVM = new HomeViewModel(control, currentWeather);
            SearchVM = new SearchViewModel();
            SettingsVM = new SettingsViewModel();

            CurrentView = HomeVM;

            SearchCommand = new RelayCommand(o =>
            {
                Response = currentWeather;
                SearchVM.UpdateControlResponse(control, Response);

                CurrentView = SearchVM;
            });

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = SearchVM;
            });

            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });
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

        public void SaveData(WeatherService control)
        {
            // = @"D:\WeatherCast\requestTime.txt"; //TODO: Сдеать обработку исключений для некоректных данных
            FileInfo fileInf = new FileInfo(Definitions.RequestTimePath);
            
            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(Definitions.RequestTimePath).ToList();
                homeCity = arrLine[0];
                lastRequestTime = DateTime.Parse(arrLine[1]);
                arrLine[0] = "Москва";

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
                lastRequestTime= DateTime.Now;

                File.Create(Definitions.RequestTimePath).Close();


                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }

            fileInf = new FileInfo(Definitions.SelectedCityInfoPath);

            if (!fileInf.Exists)
            {
                File.Create(Definitions.SelectedCityInfoPath).Close();

                currentWeather = control.GetCurrentWeather(homeCity);

                using (StreamWriter sw = File.CreateText(Definitions.SelectedCityInfoPath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, currentWeather);
                    sw.Close();
                }
            }
            else
            {
                string response;

                if((DateTime.Now - lastRequestTime).TotalMinutes >= 30)
                {
                    currentWeather = control.GetCurrentWeather(homeCity);

                    using (StreamWriter sw = File.CreateText(Definitions.SelectedCityInfoPath))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, currentWeather);
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(Definitions.SelectedCityInfoPath))
                    {
                        response = streamReader.ReadToEnd();
                        streamReader.Close();
                    }

                    currentWeather = JsonConvert.DeserializeObject<CurrentWeather>(response);
                    
                    if (currentWeather.Name.ToLower() != homeCity.ToLower())
                    {
                        currentWeather = control.GetCurrentWeather(homeCity);
                    }
                }
            }
        }
    }
}
