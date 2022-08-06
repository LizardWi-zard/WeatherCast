using System;
using WeatherCast.Core;
using WeatherCast.Model;

namespace WeatherCast.ViewModel
{
    public class SearchViewModel
    {
        public delegate void OnButtonClick(object? sender, OnButtonClick? e);

        public event OnButtonClick OnButtonClickEvent;

        public SearchViewModel()
        {
            ChangeViewCommand = new RelayCommand(o =>
            {
                OnButtonClickEvent?.Invoke(this, null);
            });
        }

        public RelayCommand ChangeViewCommand { get; set; }
    }
}
