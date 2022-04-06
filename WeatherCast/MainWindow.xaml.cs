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

    public class APIControl
    {
        private string url = "http://api.openweathermap.org/data/2.5/weather?q=Ivanovo&units=metric&appid=8b946297edc5dc36bd60f3acab86dc68";

        private HttpWebRequest httpWebRequest { get; set; }

        private HttpWebResponse httpWebResponse { get; set; }

        public string Response { get; set; }

        private void WebRequestResponse()
        {
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public WeatherResponse GetResponse()
        {
            WebRequestResponse();

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                Response = streamReader.ReadToEnd();
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(Response);
        
            return weatherResponse; 
        }

    }

    public class WeatherResponse
    {
        public string Name { get; set; }

        public TemperatureInfo Main { get; set; }
    }

    public class TemperatureInfo
    {
        public float Temp { get; set; }

        public float Feels_Like { get; set; }
    }
}










/*

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=Ivanovo&units=metric&appid=8b946297edc5dc36bd60f3acab86dc68";

            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();

            string response;
            
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
                Console.WriteLine();

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

            Console.WriteLine($"Температура сейчас: {weatherResponse.Main.Temp}");
            Console.WriteLine($"Ощущается как: {weatherResponse.Main.Feels_Like}");

            Console.ReadLine();
        }
    }

    public class WeatherResponse
    {
        public string Name { get; set; }

        public TemperatureInfo Main { get; set; }
    }

    public class TemperatureInfo
    {
        public float Temp { get; set; }

        public float Feels_Like { get; set; }
    }

*/