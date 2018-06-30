using Caliburn.Micro;
using SharpSentinel.UI.Common;
using SharpSentinel.UI.Views.About;
using SharpSentinel.UI.Views.DataSet;
using SharpSentinel.UI.Views.Settings;

namespace SharpSentinel.UI.Views
{
    public class ShellViewModel : Conductor<Screen>
    {
        private readonly IWindowManager _windowManager;

        public FluidCommand LoadDataSetCommand { get; }

        public FluidCommand OpenSettingsCommand { get; }

        public FluidCommand OpenAboutCommand { get; }

        public ShellViewModel(IWindowManager windowManager)
        {
            this._windowManager = windowManager;

            this.DisplayName = "SharpSentinel - Data Viewer";

            this.LoadDataSetCommand = FluidCommand.Sync(this.OpenDataSet);
            this.OpenSettingsCommand = FluidCommand.Sync(this.OpenSettings);
            this.OpenAboutCommand = FluidCommand.Sync(this.OpenAbout);
        }

        private void OpenAbout()
        {
            this._windowManager.ShowWindow(IoC.Get<AboutViewModel>());
        }

        private void OpenSettings()
        {
            this._windowManager.ShowWindow(IoC.Get<SettingsViewModel>());
        }

        private void OpenDataSet()
        {
            this._windowManager.ShowWindow(IoC.Get<LoadDataSetViewModel>());
        }
    }
}