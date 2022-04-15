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
        internal APIControl control;

        public HomeViewModel HomeVM { get; set; }
        
        public SearchViewModel SearchVM { get; set; }
        
        public RelayCommand HomeViewCommand { get; set; }
        
        public RelayCommand SearchViewCommand { get; set; }

        public RelayCommand GetTextCommand { get; set; }

        public WeatherResponse Response { get; set; }

        public string InputText { get; set; }

        private object _currentView;


        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; 
            OnPropertyChanged();}
        }

        public MainViewModel()
        {
            APIControl control = new APIControl();

            //Response = control.GetResponse();

            HomeVM = new HomeViewModel();
            SearchVM = new SearchViewModel(/*control, Response*/);

            CurrentView = HomeVM;

            GetTextCommand = new RelayCommand(o =>
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
