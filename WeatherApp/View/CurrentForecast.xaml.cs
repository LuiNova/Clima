using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WeatherApp.Resources;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WeatherApp.View
{
    public partial class CurrentForecast : UserControl
    {
        public CurrentForecast()
        {
            InitializeComponent();
        }

        private void ContentPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPageViewModel ViewModel = DataContext as MainPageViewModel;
            if (ViewModel != null)
            {
                //ViewModel.ShowMenu = ViewModel.ShowMenu ? false : true;
                //ViewModel.Loaded = false;
                //ViewModel.ShowCurrentMaxMin = ViewModel.ShowCurrentMaxMin ? false : true;
            }
        }

        private void ContentPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPageViewModel ViewModel = DataContext as MainPageViewModel;
            if (ViewModel != null)
            {
                //ViewModel.Loaded = false;
                //ViewModel.GetLocation();
                //MessageBox.Show(string.Format("Your location is \nLat: {0} \nLong: {1} \n", ViewModel.CurrentLocation.Latitude.ToString("0.0000"), ViewModel.CurrentLocation.Longitude.ToString("0.0000")), "Current Location", MessageBoxButton.OK);
                //ViewModel.ShowMenu = false;
                //ViewModel.ShowCurrentMaxMin = ViewModel.ShowCurrentMaxMin ? false : true;
                
                //Test
                ViewModel.Loaded = false;
                ViewModel.GetLocation();
            }
        }

        private void ContentPanel_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MainPageViewModel ViewModel = DataContext as MainPageViewModel;
            if (ViewModel != null)
            {
                //ViewModel.ShowMenu = ViewModel.ShowMenu ? false : true;
                //ViewModel.Loaded = false;
                ViewModel.ShowCurrentMaxMin = ViewModel.ShowCurrentMaxMin ? false : true;
            }
        }
    }
}
