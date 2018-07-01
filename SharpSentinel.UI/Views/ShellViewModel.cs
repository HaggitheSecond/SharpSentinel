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

        private FileTreeViewModel _fileTreeViewModel;
        private bool _isDataSetLoaded;

        public FileTreeViewModel FileTreeViewModel
        {
            get { return this._fileTreeViewModel; }
            set { this.Set(ref this._fileTreeViewModel, value); }
        }

        public bool IsDataSetLoaded
        {
            get { return this._isDataSetLoaded; }
            set { this.Set(ref this._isDataSetLoaded, value); }
        }

        public FluidCommand LoadDataSetCommand { get; }
        public FluidCommand CloseDataSetCommand { get; }
        public FluidCommand OpenSettingsCommand { get; }
        public FluidCommand OpenAboutCommand { get; }

        public ShellViewModel(IWindowManager windowManager)
        {
            this._windowManager = windowManager;

            this.DisplayName = "SharpSentinel - Data Viewer";

            this.LoadDataSetCommand = FluidCommand.Sync(this.OpenDataSet);
            this.CloseDataSetCommand = FluidCommand.Sync(this.ClostDataSet);
            this.OpenSettingsCommand = FluidCommand.Sync(this.OpenSettings);
            this.OpenAboutCommand = FluidCommand.Sync(this.OpenAbout);

            this.FileTreeViewModel = IoC.Get<FileTreeViewModel>();
        }

        private void OpenAbout()
        {
            this._windowManager.ShowDialog(IoC.Get<AboutViewModel>());
        }

        private void OpenSettings()
        {
            this._windowManager.ShowDialog(IoC.Get<SettingsViewModel>());
        }

        private void OpenDataSet()
        {
            var viewModel = IoC.Get<LoadDataSetViewModel>();

            if (this._windowManager.ShowDialog(viewModel).GetValueOrDefault() == false)
                return;
            
            this.FileTreeViewModel.Clear();
            this.FileTreeViewModel.Initialize(viewModel.LoadedData);

            this.IsDataSetLoaded = true;
        }


        private void ClostDataSet()
        {
            this.FileTreeViewModel.Clear();
            this.IsDataSetLoaded = false;
        }
    }
}