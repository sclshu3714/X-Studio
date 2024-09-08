using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace XStudio.App.Helper
{
    public static class VisualChildrenHelper
    {
        public static T? FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }
            if (obj != null && obj is T)
            {
                return (T)obj;
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                if (child == null) continue;
                T? childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }
    }
}
