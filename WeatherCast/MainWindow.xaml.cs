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

        public static object SearchText { get; internal set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchText = SearchField.Text;
        }
    }
}