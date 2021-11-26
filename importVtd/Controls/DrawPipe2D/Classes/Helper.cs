using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DrawPipe2D.Classes
{
    public class Helper
    {
        public static double HaurMinuteToRadian(double hours,double minutes)
        {
            double degree = hours*30.0 + minutes*(1.0/60.0);
            return degree*0.017453293;
        }
    }
}
