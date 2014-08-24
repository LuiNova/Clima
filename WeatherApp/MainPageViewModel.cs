using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using WeatherApp.DTO;
using Windows.Devices.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace WeatherApp
{
    /* NOTE: According to the West Midlands Public Health Observatory (UK),
     * an adequate level of warmth for older people (aged 58 and above) is 21 °C (70 °F), and 18 °C (64 °F) in other occupied rooms. 
     * 24 °C (75 °F) is stated as the maximum comfortable room temperature.
     * Owing to variations in humidity and likely clothing, recommendations for summer and winter may vary; 
     * one for summer is 23 °C (73 °F) to 24 °C (75 °F), with that for winter being 20 °C (68 °F) to 23 °C (73 °F).
     * Although by other considerations the maximum should be below 24 °C (75 °F) – for sick building syndrome avoidance, below 22 °C (72 °F).
     * 
    */
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Members and Properties

        public event PropertyChangedEventHandler PropertyChanged;

        #region Double

        private double screenWidth;
        public double ScreenWidth
        {
            get { return this.screenWidth; }
            set { this.screenWidth = value; NotifyPropertyChanged("ScreenWidth"); NotifyPropertyChanged("CanvasWidth"); NotifyPropertyChanged("NegativeScreenWidth"); }
        }

        public double NegativeScreenWidth
        {
            get { return -this.screenWidth; }
        }

        private double screenHeight;
        public double ScreenHeight
        {
            get { return this.screenHeight; }
            set { this.screenHeight = value; NotifyPropertyChanged("ScreenHeight"); NotifyPropertyChanged("CanvasHeight"); }
        }

        public double CanvasWidth
        {
            get { return 3 * this.screenWidth; }
        }

        public double CanvasHeight
        {
            get { return 100 + this.screenHeight; }
        }

        #endregion

        #region Bool

        private bool loaded;
        public bool Loaded
        {
            get { return this.loaded; }
            set { this.loaded = value; NotifyPropertyChanged("Loaded"); }
        }

        private bool showMenu;
        public bool ShowMenu
        {
            get { return this.showMenu; }
            set { this.showMenu = value; NotifyPropertyChanged("ShowMenu"); }
        }

        private bool showCurrentMaxMin;
        public bool ShowCurrentMaxMin
        {
            get { return this.showCurrentMaxMin; }
            set { this.showCurrentMaxMin = value; NotifyPropertyChanged("ShowCurrentMaxMin"); }
        }

        #endregion

        #region String

        private string apiKey;
        public string ApiKey
        {
            get { return this.apiKey; }
            set { this.apiKey = value; NotifyPropertyChanged("ApiKey"); }
        }

        private string apiKeyForecastIO;
        public string ApiKeyForecastIO
        {
            get { return this.apiKeyForecastIO; }
            set { this.apiKeyForecastIO = value; NotifyPropertyChanged("ApiKeyForecastIO"); }
        }

        #endregion

        #region DateTime

        private DateTime lastUpdated;
        public DateTime LastUpdated
        {
            get { return this.lastUpdated; }
            set { this.lastUpdated = value; NotifyPropertyChanged("LastUpdated"); }
        }

        #endregion
         
        #region Objects

        private Location currentLocation;
        public Location CurrentLocation
        {
            get { return this.currentLocation; }
            set { this.currentLocation = value; NotifyPropertyChanged("CurrentLocation"); }
        }

        private Forecast currentForecast;
        public Forecast CurrentForecast
        {
            get { return this.currentForecast; }
            set { this.currentForecast = value; NotifyPropertyChanged("CurrentForecast"); }
        }

        #endregion

        #region Collections

        private ObservableCollection<Forecast> weeklyForecast;
        public ObservableCollection<Forecast> WeeklyForecast
        {
            get { return this.weeklyForecast; }
            set { this.weeklyForecast = value; NotifyPropertyChanged("WeeklyForecast"); }
        }

        private ObservableCollection<Forecast> hourlyForecast;
        public ObservableCollection<Forecast> HourlyForecast
        {
            get { return this.hourlyForecast; }
            set { this.hourlyForecast = value; NotifyPropertyChanged("HourlyForecast"); }
        }

        #endregion

        #endregion

        #region Constructor

        public MainPageViewModel()
        {
            ApiKey = "mpnvwrbjdkpuujere2kh7kj4";
            ApiKeyForecastIO = "d1d02e99b64fd7ba063978ac19af2f04";
            ScreenWidth = System.Windows.Application.Current.Host.Content.ActualWidth;
            ScreenHeight = System.Windows.Application.Current.Host.Content.ActualHeight;
            CurrentLocation = new Location();
            CurrentForecast = new Forecast();
            WeeklyForecast = new ObservableCollection<Forecast>();
            HourlyForecast = new ObservableCollection<Forecast>();

            DateTime unix = UnixTimeStampToDateTime(1393560000);
            unix = UnixTimeStampToDateTime(1393563600);

            //Set Temperature to 200 F, to start application with grid lines of size *, and no temperatre displaying while loading
            CurrentForecast.CurrentTemperature.Temp = 200;
            CurrentForecast.MaxTemperature.Temp = 0;
            CurrentForecast.MinTemperature.Temp = 200;

            GetLocationConsent();
            //GetLocation(); //TODO: Rename methods because GetLocation() gets Forecast
            //GetForecast();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Raise the PropertyChanged event and pass along the property that changed
        /// </summary>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void GetLocationConsent()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                // User has opted in or out of Location
                return;
            }
            else
            {
                MessageBoxResult result =
                    MessageBox.Show("This application accesses your phone's location to provide weather conditions for your location. Is that ok?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        #endregion

        #region Public Methods

        public async void GetLocation()
        {
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
            {
                // The user has opted out of Location.
                MessageBox.Show("You have opted out of location. Use the app left menu to turn location back on", "Location Services", MessageBoxButton.OK); //Have gesture to turn location back on
                return;
            }

            Geolocator geolocator = new Geolocator();
            //geolocator.DesiredAccuracyInMeters = 50;
            //geolocator.MovementThreshold = 5;
            //geolocator.ReportInterval = 500;

            try
            {
                Loaded = false;
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );
                if (geoposition.CivicAddress != null)
                {
                    CurrentLocation.City = geoposition.CivicAddress.City;
                    CurrentLocation.Country = geoposition.CivicAddress.Country;
                    CurrentLocation.PostalCode = geoposition.CivicAddress.PostalCode;
                    CurrentLocation.State = geoposition.CivicAddress.State;
                }
                CurrentLocation.Latitude = geoposition.Coordinate.Latitude;
                CurrentLocation.Longitude = geoposition.Coordinate.Longitude; 

                //Testing because Geoposition is not working on my device
                //CurrentLocation.Latitude = 33.8750;
                //CurrentLocation.Longitude = -117.8900;

                //Here check Last Updated before going to get forecast and also check if location has changed by many meters.
                if (LastUpdated != null && LastUpdated != DateTime.MinValue)
                {
                    double minutes = (DateTime.Now - LastUpdated).TotalMinutes;
                    if (minutes < 10)
                    {
                        System.Threading.Thread.Sleep(3000); //This can be commented out
                        Loaded = true;
                        return;
                    }
                }

                GetForecast();
                //ChangeTileData();
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    MessageBox.Show("Location  is disabled in phone settings.", "Location Disabled", MessageBoxButton.OK);
                }
                //else
                //{
                // something else happened acquring the location
                //}
            }
        }

        /// <summary>
        /// Get a forecast for the given latitude and longitude
        /// </summary>
        public void GetForecast()
        {
            // form the URI
            //UriBuilder fullUri = new UriBuilder("http://forecast.weather.gov/MapClick.php");
            //fullUri.Query = "lat=" + CurrentLocation.Latitude.ToString("0.0000") + "&lon=" + CurrentLocation.Longitude.ToString("0.0000") + "&FcstType=dwml";
            UriBuilder fullUri = new UriBuilder("http://api.worldweatheronline.com/free/v1/weather.ashx");
            fullUri.Query = "q=" + CurrentLocation.Latitude.ToString("0.0000") + "%2C" + CurrentLocation.Longitude.ToString("0.0000") + "&format=xml&num_of_days=5&key=" + ApiKey; 

            //http://api.worldweatheronline.com/free/v1/weather.ashx?q=33.7885%2C-117.8695&format=json&num_of_days=5&key=mpnvwrbjdkpuujere2kh7kj4
            UriBuilder fullUri2 = new UriBuilder("https://api.forecast.io/forecast/" + ApiKeyForecastIO + "/" + CurrentLocation.Latitude.ToString("0.0000") + "," + CurrentLocation.Longitude.ToString("0.0000"));
            //fullUri2.Query = "q=" + CurrentLocation.Latitude.ToString("0.0000") + "%2C" + CurrentLocation.Longitude.ToString("0.0000") + "&format=json&num_of_days=5&key=" + ApiKey; 

            // initialize a new WebRequest - World Weather Online
            HttpWebRequest forecastRequest = (HttpWebRequest)WebRequest.Create(fullUri.Uri);

            //ForecastIO Request
            WebClient client = new WebClient();
            client.DownloadStringAsync(fullUri2.Uri);
            client.DownloadStringCompleted += client_DownloadStringCompletedForecast;

            // set up the state object for the async request
            ForecastUpdateState forecastState = new ForecastUpdateState();
            forecastState.AsyncRequest = forecastRequest;

            // start the asynchronous request
            forecastRequest.BeginGetResponse(new AsyncCallback(HandleForecastResponse), forecastState);
        }

        void client_DownloadStringCompletedForecast(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e != null)
            {
                WebClient client = sender as WebClient;
                //dynamic json = JsonConvert.DeserializeObject(e.Result);
                string jsonResponse = (string)e.Result;
                //var serializer = new JavaScriptSerializer();
                ForecastIOResponse forecastIO = JsonConvert.DeserializeObject<ForecastIOResponse>(jsonResponse);
                currentForecast.CurrentTemperature.Temp = (double)(forecastIO.currently.temperature);
                //Loaded = true;

                //added this code because this seems to be called twice
                if (hourlyForecast.Count == 0)
                {
                    Forecast newForecast;
                    foreach (HourForecast hourForecast in forecastIO.hourly.data)
                    {
                        newForecast = new Forecast();
                        newForecast.Date = UnixTimeStampToDateTime(hourForecast.time);
                        newForecast.CurrentTemperature.Temp = (double)(hourForecast.temperature);
                        newForecast.Description = hourForecast.summary;
                        hourlyForecast.Add(newForecast);
                    }
                }
                Loaded = true;
                ChangeTileData();
            }
        }

        /// <summary>
        /// Handle the information returned from the async request
        /// </summary>
        /// <param name="asyncResult"></param>
        private void HandleForecastResponse(IAsyncResult asyncResult)
        {
            // get the state information
            ForecastUpdateState forecastState = (ForecastUpdateState)asyncResult.AsyncState;
            HttpWebRequest forecastRequest = (HttpWebRequest)forecastState.AsyncRequest;

            // end the async request
            forecastState.AsyncResponse = (HttpWebResponse)forecastRequest.EndGetResponse(asyncResult);

            Stream streamResult;

            //string newCredit = "";
            //string newCityName = "";
            //int newHeight = 0;

            //create a temp collection for the new forecast information for each time period
            ObservableCollection<Forecast> newForecastList = new ObservableCollection<Forecast>();

            try
            {
                // get the stream containing the response from the async call
                streamResult = forecastState.AsyncResponse.GetResponseStream();

                // load the XML
                XElement xmlWeather = XElement.Load(streamResult);


                // start parsing the XML.  You can see what the XML looks like by 
                // browsing to: 
                // http://forecast.weather.gov/MapClick.php?lat=44.52160&lon=-87.98980&FcstType=dwml

                // find the source element and retrieve the credit information
                //XElement xmlCurrent = xmlWeather.Descendants("source").First();
                //newCredit = (string)(xmlCurrent.Element("credit"));

                // find the source element and retrieve the city and elevation information
                //xmlCurrent = xmlWeather.Descendants("location").First();
                //newCityName = (string)(xmlCurrent.Element("city"));
                //newHeight = (int)(xmlCurrent.Element("height"));

                double currentTemp = 200;
                double maxTemp = 0;
                double minTemp = 0;
                string condition = string.Empty;
                string dayDate = string.Empty;

                //
                XElement xmlCurrentTempCond = xmlWeather.Descendants("current_condition").First();
                currentTemp = (double)(xmlCurrentTempCond.Element("temp_F"));
                condition = (string)(xmlCurrentTempCond.Element("weatherDesc"));

                XElement xmlCurrentMaxMin = xmlWeather.Descendants("weather").First();
                maxTemp = (double)(xmlCurrentMaxMin.Element("tempMaxF"));
                minTemp = (double)(xmlCurrentMaxMin.Element("tempMinF"));
                dayDate = (string)(xmlCurrentMaxMin.Element("date"));

                //DateTime dt = Convert.ToDateTime(dayDate);
                //if (dt.Date == DateTime.Today.Date)


                //XElement xmlCurrentMin = xmlWeather.Descendants().First();
                Forecast newForecast;
                IEnumerable<XElement> xmlWeekly = xmlWeather.Descendants("weather");
                foreach (XElement element in xmlWeekly)
                {
                    newForecast = new Forecast();
                    string forecastDate = (string)(element.Element("date"));
                    newForecast.Date = Convert.ToDateTime(forecastDate);
                    newForecast.MaxTemperature.Temp = (double)(element.Element("tempMaxF"));
                    newForecast.MinTemperature.Temp = (double)(element.Element("tempMinF"));
                    newForecast.Description = (string)(element.Element("weatherDesc"));
                    newForecastList.Add(newForecast);
                }
                //foreach (XElement element in xmlCurrentTemp)
                //{
                //    if ((string)(element.Attribute("type")) == "apparent")
                //    {
                //        string value = (string)element.Element("value");
                //        if (!string.IsNullOrWhiteSpace(value))
                //        {
                //            currentTemp = (double)(element.Element("value"));
                //        }
                //        else
                //        {
                //            MessageBox.Show("Weather for your location cannot be retrived. Please change location and try again.", "Try Again!", MessageBoxButton.OK);
                //        }
                //    }
                //}

                // copy the data over
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // copy forecast object over
                    //Credit = newCredit;
                    //Height = newHeight;
                    //CityName = newCityName;

                    //ForecastList.Clear();
                    //CurrentForecast.CurrentTemperature.Temp = currentTemp;
                    //currentForecast.CurrentTemperature.Temp = currentTemp;  //Convert.ToDouble(value2);
                    currentForecast.MaxTemperature.Temp = maxTemp;
                    currentForecast.MinTemperature.Temp = minTemp;
                    currentForecast.Description = condition;
                    currentForecast.Date = Convert.ToDateTime(dayDate);

                    if (WeeklyForecast.Count != 0)
                    {
                        WeeklyForecast.Clear();
                    }
                    foreach (Forecast forecastPeriod in newForecastList)
                    {
                        forecastPeriod.ScreenWidth = ScreenWidth;
                        WeeklyForecast.Add(forecastPeriod);
                    }

                    LastUpdated = DateTime.Now;
                    //Loaded = true;
                });
            }
            catch (FormatException)
            {
                // there was some kind of error processing the response from the web
                // additional error handling would normally be added here
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                { 
                    MessageBox.Show("Weather for your location cannot be retrived. Please change location and try again.", "Try Again!", MessageBoxButton.OK);
                    Loaded = true;
                }); 
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("There was an exception: ", ex.ToString()));
            }
        } 

        public void ChangeTileData()
        {
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();
            if (tile != null)
            {
                IconicTileData tileData = new IconicTileData
                {
                    BackgroundColor = CurrentForecast.CurrentTemperature.BrushColor,
                    Count = Convert.ToInt32(CurrentForecast.CurrentTemperature.Temp)
                };
                tile.Update(tileData);
            }
        } 
         
        //public void takeScreenShot()
        //{

        //}

        //public void SaveToMediaLibrary()
        //{

        //} 

        //private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        //{
        //    var fileName = String.Format("WmDev_{0:}.jpg", DateTime.Now.Ticks);
        //    WriteableBitmap bmpCurrentScreenImage = new WriteableBitmap((int)this.ActualWidth, (int)this.ActualHeight);
        //    bmpCurrentScreenImage.Render(LayoutRoot, new MatrixTransform());
        //    bmpCurrentScreenImage.Invalidate();
        //    SaveToMediaLibrary(bmpCurrentScreenImage, fileName, 100);
        //    MessageBox.Show("Captured image " + fileName + " Saved Sucessfully", "WmDev Capture Screen", MessageBoxButton.OK);

        //    currentFileName = fileName;
        //}

        //public void SaveToMediaLibrary(WriteableBitmap bitmap, string name, int quality)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        // Save the picture to the Windows Phone media library.
        //        bitmap.SaveJpeg(stream, bitmap.PixelWidth, bitmap.PixelHeight, 0, quality);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        new MediaLibrary().SavePicture(name, stream);
        //    }
        //}

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        

        #endregion

        /// <summary>
        /// State information for our BeginGetResponse async call
        /// </summary>
        public class ForecastUpdateState
        {
            public HttpWebRequest AsyncRequest { get; set; }
            public HttpWebResponse AsyncResponse { get; set; }
        }
    }
}
