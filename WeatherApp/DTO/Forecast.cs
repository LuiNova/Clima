using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.DTO
{
    public class Forecast : INotifyPropertyChanged
    {
        #region Members and Properties

        public event PropertyChangedEventHandler PropertyChanged;

        #region Double

        private double screenWidth;
        public double ScreenWidth
        {
            get { return this.screenWidth; }
            set { this.screenWidth = value; NotifyPropertyChanged("ScreenWidth"); }
        }

        #endregion

        #region String

        private string description;
        public string Description
        {
            get { return this.description; }
            set { this.description = value; NotifyPropertyChanged("Description"); }
        }

        public string ForecastDay
        {
            get { return this.Date.DayOfWeek.ToString().Substring(0,2); }
        }

        public string ForecastDate
        {
            get
            {
                string forecastMonthDay = string.Empty;

                switch (this.Date.Month)
                {
                    case 1:
                        forecastMonthDay = "Jan";
                        break;
                    case 2:
                        forecastMonthDay = "Feb";
                        break;
                    case 3:
                        forecastMonthDay = "Mar";
                        break;
                    case 4:
                        forecastMonthDay = "Apr";
                        break;
                    case 5:
                        forecastMonthDay = "May";
                        break;
                    case 6:
                        forecastMonthDay = "Jun";
                        break;
                    case 7:
                        forecastMonthDay = "Jul";
                        break;
                    case 8:
                        forecastMonthDay = "Aug";
                        break;
                    case 9:
                        forecastMonthDay = "Sep";
                        break;
                    case 10:
                        forecastMonthDay = "Oct";
                        break;
                    case 11:
                        forecastMonthDay = "Nov";
                        break;
                    case 12:
                        forecastMonthDay = "Dec";
                        break;
                }

                forecastMonthDay += " " + this.Date.Day.ToString();

                return forecastMonthDay;
            }
        }

        #endregion

        #region Objects

        private Temperature currentTemperature;
        public Temperature CurrentTemperature
        {
            get { return this.currentTemperature; }
            set { this.currentTemperature = value; NotifyPropertyChanged("CurrentTemperature"); }
        }

        private Temperature maxTemperature;
        public Temperature MaxTemperature
        {
            get { return this.maxTemperature; }
            set { this.maxTemperature = value; NotifyPropertyChanged("MaxTemperature"); }
        }

        private Temperature minTemperature;
        public Temperature MinTemperature
        {
            get { return this.minTemperature; }
            set { this.minTemperature = value; NotifyPropertyChanged("MinTemperature"); }
        }

        #endregion

        #region DateTime

        private DateTime date;
        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; NotifyPropertyChanged("Date"); NotifyPropertyChanged("ForecastDate"); NotifyPropertyChanged("ForecastDay"); }
        }

        #endregion

        #endregion

        #region Constructor

        public Forecast()
        {
            CurrentTemperature = new Temperature();
            MaxTemperature = new Temperature();
            MinTemperature = new Temperature();
        }

        #endregion

        #region Methods

        #region Private

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

        #endregion

        #region Public

        public Forecast Copy()
        {
            Forecast copy = new Forecast();

            copy.CurrentTemperature = this.CurrentTemperature;
            copy.MaxTemperature = this.MaxTemperature;
            copy.MinTemperature = this.MinTemperature;
            copy.Date = this.Date;
            copy.Description = this.Description;

            return copy;
        }

        #endregion

        #endregion
    }
}
