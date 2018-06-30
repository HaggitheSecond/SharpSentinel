using System;
using System.Collections.Generic;
using System.Reflection;
using MahApps.Metro.Controls;

namespace SharpSentinel.UI.Services.Window
{
    public class WindowManager : Caliburn.Micro.WindowManager
    {
        protected override System.Windows.Window EnsureWindow(object model, object view, bool isDialog)
        {
            return new MetroWindow
            {
                Content = view
            };
        }

        protected override System.Windows.Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            var window = base.CreateWindow(rootModel, isDialog, context, null);

            if (!(window.Content is IWindow viewWithSettings))
                throw new Exception("Missing implementation of IWindow: " +  window.Content);

            if(this.ApplySettings(window, viewWithSettings.GetWindowSettings()) == false)
                throw new Exception("Unable to apply settings");

            return window;
        }

        // this is a copy of the apply settings in Caliburn.Micro.WindowManager as it is not overrideable
        // but we need to be able to call it with our own settings from createwindow
        private bool ApplySettings(object target, IEnumerable<KeyValuePair<string, object>> settings)
        {
            if (settings == null)
                return false;

            var type = target.GetType();
            foreach (var setting in settings)
            {
                var property = type.GetProperty(setting.Key);
                if (property != null)
                    property.SetValue(target, setting.Value, null);
            }
            return true;
        }
    }
}