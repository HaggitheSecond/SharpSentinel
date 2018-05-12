using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SharpSentinel.UI.Services.Window
{
    public class WindowSettings : Dictionary<string, object>
    {
        private WindowSettings()
        {

        }

        public static WindowSettings Standard()
        {
            return new WindowSettings()
                .SizeToContent(System.Windows.SizeToContent.WidthAndHeight)
                .Resize(ResizeMode.CanResizeWithGrip)
                .WindowStartupLocation(System.Windows.WindowStartupLocation.CenterScreen)
                .MinHeight(200)
                .MinWidth(100);
        }

        public WindowSettings Icon(Uri path)
        {
            this[nameof(System.Windows.Window.Icon)] = new BitmapImage(path);
            return this;
        }

        public WindowSettings Width(int width)
        {
            this[nameof(System.Windows.Window.Width)] = width;
            return this;
        }

        public WindowSettings Height(int height)
        {
            this[nameof(System.Windows.Window.Height)] = height;
            return this;
        }

        public WindowSettings SizeToContent(SizeToContent sizeToContent)
        {
            this[nameof(System.Windows.Window.SizeToContent)] = sizeToContent;
            return this;
        }

        public WindowSettings Resize(ResizeMode mode)
        {
            this[nameof(System.Windows.Window.ResizeMode)] = mode;
            return this;
        }

        public WindowSettings WindowStartupLocation(WindowStartupLocation location)
        {
            this[nameof(System.Windows.Window.WindowStartupLocation)] = location;
            return this;
        }

        public WindowSettings Left(double left)
        {
            this[nameof(System.Windows.Window.Left)] = left;
            return this;
        }

        public WindowSettings Top(double top)
        {
            this[nameof(System.Windows.Window.Top)] = top;
            return this;
        }

        public WindowSettings WindowState(WindowState state)
        {
            this[nameof(System.Windows.Window.WindowState)] = state;
            this[nameof(System.Windows.Window.SizeToContent)] = System.Windows.SizeToContent.Manual;
            return this;
        }

        public WindowSettings MinWidth(int width)
        {
            this[nameof(System.Windows.Window.MinWidth)] = width;
            return this;
        }

        public WindowSettings MinHeight(int height)
        {
            this[nameof(System.Windows.Window.MinHeight)] = height;
            return this;
        }

        public WindowSettings Background(SolidColorBrush color)
        {
            if (color.Color == Colors.Transparent)
            {
                this[nameof(System.Windows.Window.WindowStyle)] = System.Windows.WindowStyle.None;
                this[nameof(System.Windows.Window.AllowsTransparency)] = true;
            }

            this[nameof(System.Windows.Window.Background)] = color;
            return this;
        }

        public WindowSettings HitTestVisible(bool hitTestVisible)
        {
            this[nameof(System.Windows.Window.IsHitTestVisible)] = hitTestVisible;
            return this;
        }

        public WindowSettings WindowStyle(WindowStyle style)
        {
            this[nameof(System.Windows.Window.WindowStyle)] = style;
            return this;
        }

        public WindowSettings IsTopMost()
        {
            this[nameof(System.Windows.Window.Topmost)] = true;
            return this;
        }
    }
}