using System;
using System.Collections.Generic;
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
        private string homeCity = "Ivanovo";
        private WeatherService control;

        public HomeViewModel HomeVM { get; set; }

        public SearchViewModel SearchVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand SearchViewCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public CurrentWeather Response { get; set; }

        public string InputText { get; set; }

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
            currentWeather = control.GetCurrentWeather(homeCity);


            HomeVM = new HomeViewModel(control, currentWeather);
            SearchVM = new SearchViewModel();

            CurrentView = HomeVM;

            SearchCommand = new RelayCommand(o =>
            {
                //InputText = MainWindow.SearchText.ToString(); //TODO: Сделать обработку логики для поиска
                //APICall(control);
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

        //void APICall(WeatherService control)
        //{
        //    control.CreateСurrentWeatherUrl(InputText);
        //}
    }
}
