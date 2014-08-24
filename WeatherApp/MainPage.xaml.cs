using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WeatherApp.Resources;

using System.IO.IsolatedStorage;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Windows.Media.Animation;

namespace WeatherApp
{
    public partial class MainPage : PhoneApplicationPage
    {

        #region Members and Properties

        MainPageViewModel ViewModel
        {
            get
            {
                return (MainPageViewModel)DataContext;
            }
        }

        double initialPosition;
        bool _viewMoved = false;

        #endregion

        #region Constructor

        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageViewModel();

            //This is to set the controls in the right position of Canvas for swiping
            this.ucCurrentForecast.Margin = new Thickness(ViewModel.ScreenWidth, 0, 0, 0);
            this.ucWeeklyForecast.Margin = new Thickness(ViewModel.ScreenWidth * 2, 0, 0, 0);
        }

        #endregion

        #region Private Methods

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.GetLocation();
            }
        }

        void MoveViewWindow(double left)
        {
            _viewMoved = true;
            ((Storyboard)canvas.Resources["moveAnimation"]).SkipToFill();
            ((DoubleAnimation)((Storyboard)canvas.Resources["moveAnimation"]).Children[0]).To = left;
            ((Storyboard)canvas.Resources["moveAnimation"]).Begin();
        }

        #endregion

        private void canvas_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Loaded = false;
                ViewModel.GetLocation();
            }
        }

        private void Reload_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModel != null)
            {
                //ViewModel.ShowMenu = false;
                ViewModel.Loaded = false;
                ViewModel.ShowMenu = ViewModel.ShowMenu ? false : true;
                ViewModel.GetLocation();
            }
        }

        private void Weekly_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.ShowCurrentMaxMin = false;
                ViewModel.ShowMenu = ViewModel.ShowMenu ? false : true;
                this.ucCurrentForecast.Visibility = this.ucCurrentForecast.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                this.ucWeeklyForecast.Visibility = this.ucWeeklyForecast.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }
        }

        private void ShowMaxMin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.ShowCurrentMaxMin = ViewModel.ShowCurrentMaxMin ? false : true;
            }
        }

        private void canvas_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (ViewModel.Loaded)
            {
                _viewMoved = false;
                initialPosition = Canvas.GetLeft(LayoutRoot);
            }
        }

        private void canvas_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (ViewModel.Loaded)
            {
                if (e.DeltaManipulation.Translation.X != 0) //960
                    Canvas.SetLeft(LayoutRoot, Math.Min(Math.Max(-(2 * ViewModel.ScreenWidth), Canvas.GetLeft(LayoutRoot) + e.DeltaManipulation.Translation.X), 0));
            }
        }

        private void canvas_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (ViewModel.Loaded)
            {
                var left = Canvas.GetLeft(LayoutRoot);
                if (_viewMoved)
                    return;
                if (Math.Abs(initialPosition - left) < 100)
                {
                    //bouncing back
                    MoveViewWindow(initialPosition);
                    return;
                }
                //change of state
                if (initialPosition - left > 0)
                {
                    //slide to the left
                    if (initialPosition > -ViewModel.ScreenWidth) //480
                        MoveViewWindow(-ViewModel.ScreenWidth); //480
                    else
                        MoveViewWindow(-(2 * ViewModel.ScreenWidth));   //960
                }
                else
                {
                    //slide to the right
                    if (initialPosition < -ViewModel.ScreenWidth) //480
                        MoveViewWindow(-ViewModel.ScreenWidth);   //480
                    else
                        MoveViewWindow(0);
                }
            }
        }

        #region Public Methods

        #endregion 
    }
}