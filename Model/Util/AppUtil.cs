using System;
using System.Net;
using System.Windows;
using System.Windows.Media;

namespace FingerPrintManagerApp.Model.Util
{
    public class AppUtil
    {
        public static string GetClientIPAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(hostName);

            foreach (var address in entry.AddressList)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }

            return string.Empty;
        }

        public static string GetClientMachineName()
        {
            return Environment.MachineName;
        }

        public static void CloseAllDialogs()
        {
            var collects = App.Current.Windows;

            foreach (Window win in collects)
            {
                if (win.Name != "MainWindow")
                {
                    win.Close();
                }
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }
    }
}
