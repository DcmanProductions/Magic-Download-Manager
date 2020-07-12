using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public static class UIUtility
    {
        public static bool ToggleCheckBox(Button button)
        {
            DockPanel dock = button.Content.GetType().Equals(typeof(DockPanel)) ? (DockPanel)button.Content : null;
            Image image = null;
            TextBlock text = null;
            if (dock != null)
            {
                foreach (var element in dock.Children)
                {
                    if (element.GetType().Equals(typeof(Image)))
                    {
                        image = (Image)element;
                    }
                    else if (element.GetType().Equals(typeof(TextBlock)))
                    {
                        text = (TextBlock)element;
                    }
                }
            }
            bool value = image.Source == null ? true : false;
            image.Source = value ? new BitmapImage(new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/Transparent/Check Mark.png")) : null;
            return value;
        }



        public static void SetCheckBox(Button button, bool value)
        {
            DockPanel dock = button.Content.GetType().Equals(typeof(DockPanel)) ? (DockPanel)button.Content : null;
            Image image = null;
            TextBlock text = null;
            if (dock != null)
            {
                foreach (var element in dock.Children)
                {
                    if (element.GetType().Equals(typeof(Image)))
                    {
                        image = (Image)element;
                    }
                    else if (element.GetType().Equals(typeof(TextBlock)))
                    {
                        text = (TextBlock)element;
                    }
                }
            }
            image.Source = value ? new BitmapImage(new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Resources/Transparent/Check Mark.png")) : null;
        }

    }
}
