using System.Linq;
using WeatherCast.Core;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            SendTextCommand = new RelayCommand(o =>
            {
                SelectedCity = InputText;
            });
        }

        public string SelectedCity { get; set; }

        public string InputText { get; set; }

        public RelayCommand SendTextCommand { get; set; } 

    }
}
