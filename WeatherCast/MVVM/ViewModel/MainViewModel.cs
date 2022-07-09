using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCast.Core;

namespace WeatherCast.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private CurrentWeather currentWeather;
        private object _currentView;
        private WeatherService control;
        private DateTime lastRequestTime;
        private string homeCity = "Иваново";

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

        public MainViewModel()
        {
            WeatherService control = new WeatherService();

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
            
            // ты можешь использовать Path.Combine для безопасного "склеивания" частей пути
            //string pathToSave = currentDirectory + @"\\requestTime.txt";

            string pathToSave = Path.Combine(currentDirectory, "requestTime.txt");
            FileInfo fileInf = new FileInfo(pathToSave);
            
            List<string> arrLine = new List<string>();

            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(@"D:\WeatherCast\requestTime.txt").ToList();
                homeCity = arrLine[0];
                lastRequestTime = DateTime.Parse(arrLine[1]);
                arrLine[0] = "Иваново";

                if ((DateTime.Now - lastRequestTime).TotalMinutes >= 1)
                {
                    arrLine[1] = DateTime.Now.ToString();
                }

                File.WriteAllLines(@"D:\WeatherCast\requestTime.txt", arrLine);
            }
            else
            {
                arrLine.Add("Иваново");
                arrLine.Add(DateTime.Now.ToString());
                
                homeCity = "Иваново";
                lastRequestTime= DateTime.Now;

                File.Create(@"D:\WeatherCast\requestTime.txt").Close();
                File.WriteAllLines(@"D:\WeatherCast\requestTime.txt", arrLine);
            }

            pathToSave = Path.Combine(currentDirectory, "SelectedCityInfo.txt");
            
            fileInf = new FileInfo(pathToSave);

            if (!fileInf.Exists)
            {
                File.Create(path).Close();

                currentWeather = control.GetCurrentWeather(homeCity);

                using (StreamWriter sw = File.CreateText(path))
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

                    using (StreamWriter sw = File.CreateText(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(sw, currentWeather);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(path))
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
