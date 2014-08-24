using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.DTO
{
    public class ForecastPeriod : INotifyPropertyChanged
    {
        #region Members and Properties

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public ForecastPeriod()
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

        public ForecastPeriod Copy()
        {
            ForecastPeriod copy = new ForecastPeriod();

            return copy;
        }

        #endregion

        #endregion
    }
}
