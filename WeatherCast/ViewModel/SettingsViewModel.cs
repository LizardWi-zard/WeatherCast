using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherCast.ViewModel
{
    public class SettingsViewModel
    {
        private string selectedCity;

        public string SelectedCity
        {
            get { return this.selectedCity; }
            set
            {
                if (!string.Equals(this.selectedCity, value))
                {
                    this.selectedCity = value;
                }
            }
        }

    }
}
