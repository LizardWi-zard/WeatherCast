using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Windows;

namespace WeatherCast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            APIControl control = new APIControl();

            WeatherResponse response = control.GetResponse();

            MessageBox.Show($"Температура : {response.Main.Temp} \t Ощущается как : {response.Main.Feels_Like}");
        }
    }
}