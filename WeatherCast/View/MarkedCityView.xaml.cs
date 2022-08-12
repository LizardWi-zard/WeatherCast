using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherCast.Helpers;

namespace WeatherCast.View
{
    /// <summary>
    /// Логика взаимодействия для SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void OpenSearchWindow(object sender, RoutedEventArgs e)
        {
            SearchWindow window = new SearchWindow();

            window.ShowDialog();

            SettingsProvider singleton = SettingsProvider.getInstance();

            if (window.DialogResult == true)
            {
                singleton.AddCity(window.CityName);
            }
            
        }

        private void List_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                MarkedPageScrollViewer.LineDown(); // Использую два раза чтоб скорость прокрутки была одинаковая как вне так и внутри ListBox
                MarkedPageScrollViewer.LineDown();
            }
            else
            {
                MarkedPageScrollViewer.LineUp();
                MarkedPageScrollViewer.LineUp();
            }
        }
    }
}
