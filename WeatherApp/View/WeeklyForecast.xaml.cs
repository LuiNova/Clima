using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WeatherApp.View
{
    public partial class WeeklyForecast : UserControl
    {
        public WeeklyForecast()
        {
            InitializeComponent();
        }

        private void ContentPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPageViewModel ViewModel = DataContext as MainPageViewModel;
            if (ViewModel != null)
            {
                ViewModel.ShowMenu = ViewModel.ShowMenu ? false : true;
            }
        }

        private void ContentPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPageViewModel ViewModel = DataContext as MainPageViewModel;
            if (ViewModel != null)
            {
                ViewModel.Loaded = false;
                ViewModel.GetLocation();
            }
        }
    }
}
