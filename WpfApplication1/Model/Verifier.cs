using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApplication1.Model
{
    static class Verifier
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static bool SwitchControls(this IEnumerable<FrameworkElement> elements, bool flag)
        {
            foreach (var element in elements)
            {
                element.IsEnabled = flag;
            }
            //Parallel.ForEach(elements, (element) =>
            //{
            //    element.IsEnabled = flag;
            //});
            return flag;
        }


        public static void StringNullСorrection(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                s = "0";
            }
        }

        public static void StringNullСorrection(params string[] strAr)
        {
            for (int i = 0; i < strAr.Length; i++)
            {
                StringNullСorrection(strAr[i]);
            }
        }
    }
}
