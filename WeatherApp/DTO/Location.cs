using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.DTO
{
    public class Location : INotifyPropertyChanged
    {
        #region Members and Properties

        public event PropertyChangedEventHandler PropertyChanged;

        private string city;
        public string City
        {
            get { return this.city; }
            set { this.city = value; NotifyPropertyChanged("City"); }
        }

        private string country;
        public string Country
        {
            get { return this.country; }
            set { this.country = value; NotifyPropertyChanged("Country"); }
        }

        private string postalCode;
        public string PostalCode
        {
            get { return this.postalCode; }
            set { this.postalCode = value; NotifyPropertyChanged("PostalCode"); }
        }

        private string state;
        public string State
        {
            get { return this.state; }
            set { this.state = value; NotifyPropertyChanged("State"); }
        }

        private double latitude;
        public double Latitude
        {
            get { return this.latitude; }
            set { this.latitude = value; NotifyPropertyChanged("Latitude"); }
        }

        private double longitude;
        public double Longitude
        {
            get { return this.longitude; }
            set { this.longitude = value; NotifyPropertyChanged("Longitude"); }
        }

        #endregion

        #region Constructor

        public Location()
        {
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

        public Location Copy()
        {
            Location copy = new Location();

            copy.City = this.City;
            copy.Country = this.Country;
            copy.Latitude = this.Latitude;
            copy.Longitude = this.Longitude;
            copy.PostalCode = this.PostalCode;
            copy.State = this.State;

            return copy;
        }

        #endregion

        #endregion
    }
}
