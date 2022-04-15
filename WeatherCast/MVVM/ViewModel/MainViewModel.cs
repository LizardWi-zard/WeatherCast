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
        public HomeViewModel HomeVM { get; set; }
        
        public SearchViewModel SearchVM { get; set; }
        
        public RelayCommand HomeViewCommand { get; set; }
        
        public RelayCommand SearchViewCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public WeatherResponse Response { get; set; }

        public string InputText { get; set; }

        private object _currentView;
        APIControl control;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; 
            OnPropertyChanged();}
        }

        public MainViewModel()
        {
            APIControl control = new APIControl();

            HomeVM = new HomeViewModel();
            SearchVM = new SearchViewModel();

            CurrentView = HomeVM;

            SearchCommand = new RelayCommand(o =>
            {
                InputText = MainWindow.SearchText.ToString();
                APICall(control);
                Response = control.GetResponse();
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

        void APICall(APIControl control)
        {
            control.CreateAPIurl(InputText);
        }
    }
}
