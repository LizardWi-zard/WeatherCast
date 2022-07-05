using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
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
        private string currentDirectory;

        public MainViewModel()
        {
            WeatherService control = new WeatherService();

            currentDirectory = Directory.GetCurrentDirectory();
            
            SaveData(control);

            VMBase = new ViewModelBase(control, currentWeather);

            HomeVM = new HomeViewModel(control, currentWeather);
            SearchVM = new SearchViewModel();

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

        }

        public ViewModelBase VMBase { get; set; }

        public HomeViewModel HomeVM { get; set; }

        public SearchViewModel SearchVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand SearchViewCommand { get; set; }

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
            
            string pathToSave = currentDirectory + @"\\requestTime.txt"; 
            FileInfo fileInf = new FileInfo(pathToSave);
            
            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(pathToSave).ToList();
                homeCity = arrLine[0];
                lastRequestTime = DateTime.Parse(arrLine[1]);
                arrLine[0] = "Москва";

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
                lastRequestTime= DateTime.Now;

                File.Create(pathToSave).Close();
                
                //сделать универсальный относительный путь рядом с приложением, пример:
                //string currentDirectory = Assembly.GetEntryAssembly().Location;
                //string pathToSave = Path.Combine(currentDirectory, "requestTime.txt");

                File.WriteAllLines(pathToSave, arrLine);
            }

            pathToSave = currentDirectory + @"\\SelectedCityInfo.txt";
            
            fileInf = new FileInfo(pathToSave);

            if (!fileInf.Exists)
            {
                File.Create(pathToSave).Close();

                currentWeather = control.GetCurrentWeather(homeCity);

                using (StreamWriter sw = File.CreateText(pathToSave))
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

                    using (StreamWriter sw = File.CreateText(pathToSave))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, currentWeather);
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(pathToSave))
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
