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

namespace WeatherCast.View
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void List_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                MainScrollViewer.LineDown(); // Использую два раза чтоб скорость прокрутки была одинаковая как вне так и внутри ListBox
                MainScrollViewer.LineDown();
            }
            else
            {
                MainScrollViewer.LineUp();
                MainScrollViewer.LineUp();
            }
        }

        private void Click_Daily_ScrollLeft(object sender, RoutedEventArgs e)
        {
            ScrollHorizontally(-150.0, DailyList);
        }

        private void Click_Daily_ScrollRight(object sender, RoutedEventArgs e)
        {
            ScrollHorizontally(150.0, DailyList);
        }

        private void Click_Hourly_ScrollLeft(object sender, RoutedEventArgs e)
        {
            ScrollHorizontally(-150.0, HourlyList);
        }

        private void Click_Hourly_ScrollRight(object sender, RoutedEventArgs e)
        {
            ScrollHorizontally(150.0, HourlyList);
        }

        private void ScrollHorizontally(double offset, ListBox listBox)
        {
            Decorator border = VisualTreeHelper.GetChild(listBox, 0) as Decorator;

            ScrollViewer scrollViewer = border.Child as ScrollViewer;

            scrollViewer?.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + offset);
        }
    }
}
