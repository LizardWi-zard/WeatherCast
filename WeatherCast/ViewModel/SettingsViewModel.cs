using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeatherCast.Core;
using WeatherCast.Model;
using WeatherCast.Helpers;
using Newtonsoft.Json;
using WeatherCast.DataProvider;
using System.Windows;

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

                    try
                    {
                        var lastRequestInfo = new LastRequestCurrentInfo()
                        {
                            CityName = SelectedCity
                        };

                        using (StreamWriter sw = File.CreateText(Definitions.RequestCurrentInfoTimePath))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(sw, lastRequestInfo);
                            sw.Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("не удалось сохранить");
                    }
                }
            });
        }

        public string SelectedCity { get; set; }

        public string InputText { get; set; }

        public RelayCommand SendTextCommand { get; set; } 
    }
}
