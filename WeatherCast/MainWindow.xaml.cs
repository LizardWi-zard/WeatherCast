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
            string city = SearchField.Text;

            APIControl control = new APIControl();

            control.CreateAPIurl(city);

            WeatherResponse response = control.GetResponse();

            if (response != null)
            {
                MessageBox.Show($"Температура : {response.Main.Temp} \t Ощущается как : {response.Main.Feels_Like}");
            }
        }
    }
}