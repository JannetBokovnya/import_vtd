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
    public class VisualHelper
    {
        public static T FindVisualParent<T>(DependencyObject element) where T : class
        {
            while (element != null)
            {
                if (element is T)
                    return element as T;

                element = VisualTreeHelper.GetParent(element);
            }
            return null;
        }

        public static T FindVisualChild<T>(DependencyObject element) where T : class
        {
            while (element != null)
            {
                if (element is T)
                    return element as T;

                element = VisualTreeHelper.GetChild(element, 0);
            }
            return null;
        }
    }
}
