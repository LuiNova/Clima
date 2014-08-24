using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WeatherApp.DTO
{
    public class Temperature : INotifyPropertyChanged
    {
        #region Members and Properties

        public event PropertyChangedEventHandler PropertyChanged;

        #region Bool

        //Above Adequate Temperatures
        //105 < Temperature < 200
        public bool IsSuperHot
        {
            get { return ((Temp > 105) && (Temp < 200)); }
        }

        // 100 < Temperature <= 105
        public bool IsHotter
        {
            get { return ((Temp > 100) && (Temp <= 105)); }
        }

        //95 < Temperature <= 100
        public bool IsABitHotter
        {
            get { return ((Temp > 95) && (Temp <= 100)); }
        }

        //90 < Temperature <= 95
        public bool IsHot
        {
            get { return ((Temp > 90) && (Temp <= 95)); }
        }

        //85 < Temperature <= 90
        public bool IsABitHot
        {
            get { return ((Temp > 85) && (Temp <= 90)); }
        }

        //80 < Temperature <= 85
        public bool IsWarmer
        {
            get { return ((Temp > 80) && (Temp <= 85)); }
        }

        //75 < Temperature <= 80
        public bool IsWarm
        {
            get { return ((Temp > 75) && (Temp <= 80)); }
        }
        //Above Adequate Temperatures - End 

        //70 <= Temperature <= 75
        public bool IsAdequateTemp
        {
            get { return ((Temp >= 70) && (Temp <= 75)); }
        }

        //Below Adequate Temperatures 

        //65 <= Temperature < 70
        public bool IsCool
        {
            get { return ((Temp >= 65) && (Temp < 70)); }
        }

        //60 <= Temperature < 65
        public bool IsCooler
        {
            get { return ((Temp >= 60) && (Temp < 65)); }
        }

        //55 <= Temperature < 60
        public bool IsABitCold
        {
            get { return ((Temp >= 55) && (Temp < 60)); }
        }

        //50 <= Temperature < 55
        public bool IsCold
        {
            get { return ((Temp >= 50) && (Temp < 55)); }
        }

        //45 <= Temperature < 50
        public bool IsABitColder
        {
            get { return ((Temp >= 45) && (Temp < 50)); }
        }

        //40 <= Temperature < 45
        public bool IsColder
        {
            get { return ((Temp >= 40) && (Temp < 45)); }
        }

        //Temperature < 40
        public bool IsSuperCold
        {
            get { return (Temp < 40); }
        }
        //Below Adequate Temperatures - End 

        #endregion

        #region Double

        private double temp;
        public double Temp
        {
            get { return this.temp; }
            set
            {
                this.temp = value;
                NotifyPropertyChanged("Temp");
                NotifyPropertyChanged("IsSuperHot");
                NotifyPropertyChanged("IsHotter");
                NotifyPropertyChanged("IsABitHotter");
                NotifyPropertyChanged("IsHot");
                NotifyPropertyChanged("IsABitHot");
                NotifyPropertyChanged("IsWarmer");
                NotifyPropertyChanged("IsWarm");
                NotifyPropertyChanged("IsAdequateTemp");
                NotifyPropertyChanged("IsCool");
                NotifyPropertyChanged("IsCooler");
                NotifyPropertyChanged("IsABitCold");
                NotifyPropertyChanged("IsCold");
                NotifyPropertyChanged("IsABitColder");
                NotifyPropertyChanged("IsColder");
                NotifyPropertyChanged("IsSuperCold");
                NotifyPropertyChanged("BrushColor");
                NotifyPropertyChanged("BrushColorString");
            }
        }

        #endregion

        #region Color

        public Color BrushColor
        {
            get
            {
                Color currentBrush = Colors.White;

                if (IsSuperHot) 
                {
                    currentBrush = Color.FromArgb(255, 243, 100, 18);
                }
                else if (IsHotter) 
                {
                    currentBrush = Color.FromArgb(255, 243, 137, 18);
                }
                else if (IsABitHotter) 
                {
                    currentBrush = Color.FromArgb(255, 243, 175, 18);
                }
                else if (IsHot)
                {
                    currentBrush = Color.FromArgb(255, 243, 212, 18);
                }
                else if (IsABitHot)
                {
                    currentBrush = Color.FromArgb(255, 246, 222, 74);
                }
                else if (IsWarmer)
                {
                    currentBrush = Color.FromArgb(255, 249, 232, 130);
                }
                else if (IsWarm)
                {
                    currentBrush = Color.FromArgb(255, 252, 242, 186);
                }
                else if (IsAdequateTemp)
                {
                    currentBrush = Colors.White;
                }
                else if (IsCool)
                {
                    currentBrush = Color.FromArgb(255, 231, 242, 250);
                }
                else if (IsCooler)
                {
                    currentBrush = Color.FromArgb(255, 199, 225, 243);
                }
                else if (IsABitCold) 
                {
                    currentBrush = Color.FromArgb(255, 167, 208, 236);
                }
                else if (IsCold)
                {
                    currentBrush = Color.FromArgb(255, 135, 191, 228);
                }
                else if (IsABitColder)
                {
                    currentBrush = Color.FromArgb(255, 103, 174, 221);
                }
                else if (IsColder)
                {
                    currentBrush = Color.FromArgb(255, 71, 157, 214);
                }
                else if (IsSuperCold)
                {
                    currentBrush = Color.FromArgb(255, 41, 128, 185);
                }

                string currentBrushString = currentBrush.ToString();
                return currentBrush;
            }
        }

        #endregion

        #region String

        public string BrushColorString
        {
            get { return this.BrushColor.ToString(); }
        }

        #endregion

        #endregion

        #region Constructor

        public Temperature()
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

        public Temperature Copy()
        {
            Temperature copy = new Temperature();

            copy.Temp = this.Temp;

            return copy;
        }

        #endregion

        #endregion
    }
}
