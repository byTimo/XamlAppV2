using System.Windows;
using System.Windows.Media;
using NLog;

namespace ZappChat_v3.Core
{
    public static class Support
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

    }
}