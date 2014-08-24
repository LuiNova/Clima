using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WeatherApp.DTO
{
    public class ClimaColorPalette
    {
        public Color warm7 = Color.FromArgb(255, 243, 100, 18);
        public Color warm6 = Color.FromArgb(255, 243, 137, 18);
        public Color warm5 = Color.FromArgb(255, 243, 175, 18);
        public Color warm4 = Color.FromArgb(255, 243, 212, 18);
        public Color warm3 = Color.FromArgb(255, 246, 222, 74);
        public Color warm2 = Color.FromArgb(255, 249, 232, 130);
        public Color warm = Color.FromArgb(255, 252, 242, 186);
        public Color adequate = Colors.White;
        public Color cool = Color.FromArgb(255, 231, 242, 250);
        public Color cool2 = Color.FromArgb(255, 199, 255, 243);
        public Color cool3 = Color.FromArgb(255, 167, 208, 236);
        public Color cool4 = Color.FromArgb(255, 135, 191, 228);
        public Color cool5 = Color.FromArgb(255, 103, 174, 221);
        public Color cool6 = Color.FromArgb(255, 71, 157, 214);
        public Color cool7 = Color.FromArgb(255, 41, 128, 185);
    }
}
