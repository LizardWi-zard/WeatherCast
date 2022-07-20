using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherCast.Core;
using WeatherCast.Model;
using WeatherCast.Helpers;

namespace WeatherCast.ViewModel
{
    public class SettingsViewModel 
    {
        public SettingsViewModel(string city)
        {
            InputText = city;

            SendTextCommand = new RelayCommand(o =>
            {
                if (!(string.IsNullOrWhiteSpace(InputText) || !(InputText.All(c => Char.IsLetter(c) || c == '-') && InputText.Count(f => f == '-') < 2 && InputText.Length > 1)))
                {
                    SelectedCity = !string.IsNullOrWhiteSpace(InputText) ? InputText : Definitions.DefaultCity;

                    OverWriteDefaultCity();
                }
            });
        }

        public string SelectedCity { get; set; }

        public string InputText { get; set; }

        public RelayCommand SendTextCommand { get; set; } 

        public void OverWriteDefaultCity()
        {
            FileInfo fileInf = new FileInfo(Definitions.RequestTimePath);

            List<string> arrLine = new List<string>();


            if (fileInf.Exists)
            {
                arrLine = File.ReadAllLines(Definitions.RequestTimePath).ToList();
                arrLine[0] = SelectedCity;

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }
            else
            {
                arrLine.Add(SelectedCity);
                arrLine.Add(DateTime.Now.ToString());

                File.Create(Definitions.RequestTimePath).Close();

                File.WriteAllLines(Definitions.RequestTimePath, arrLine);
            }
        }
    }
}
