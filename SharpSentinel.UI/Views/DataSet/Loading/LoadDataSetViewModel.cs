﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using SharpSentinel.Parser.Data;
using SharpSentinel.Parser.Helpers;
using SharpSentinel.UI.Common;
using SharpSentinel.UI.Services.Messages;
using SharpSentinel.UI.Services.Parser;

namespace SharpSentinel.UI.Views.DataSet
{
    public class LoadDataSetViewModel : Screen
    {
        private readonly IMessageManager _messageManager;
        private readonly IParsingService _parsingService;

        private string _selectedFolderPath;
        private BaseData _loadedData;

        private bool? _isValidFolder;
        private string _selectedFolderErrorMessage;

        public string SelectedFolderPath
        {
            get { return this._selectedFolderPath; }
            set { this.Set(ref this._selectedFolderPath, value); }
        }

        public BaseData LoadedData
        {
            get { return this._loadedData; }
            set { this.Set(ref this._loadedData, value); }
        }

        public bool? IsValidFolder
        {
            get { return this._isValidFolder; }
            set { this.Set(ref this._isValidFolder, value); }
        }

        public string SelectedFolderErrorMessage
        {
            get { return this._selectedFolderErrorMessage; }
            set { this.Set(ref this._selectedFolderErrorMessage, value); }
        }

        public FluidCommand SelectFolderCommand { get; }

        public FluidCommand CancelCommand { get; }
        public FluidCommand LoadDataSetCommand { get; }

        public LoadDataSetViewModel(IMessageManager messageManager, IParsingService parsingService)
        {
            this._messageManager = messageManager;
            this._parsingService = parsingService;

            this.DisplayName = "SharpSentinel - open data set";

            this.SelectFolderCommand = FluidCommand.Sync(this.SelectFolder);

            this.CancelCommand = FluidCommand.Sync(() => this.TryClose(false));
            this.LoadDataSetCommand = FluidCommand.Async(this.LoadDataSet).CanExecute(this.CanLoadDataSet);
        }

        private bool CanLoadDataSet()
        {
            return string.IsNullOrWhiteSpace(this.SelectedFolderPath) == false && this.IsValidFolder.GetValueOrDefault();
        }

        private async Task LoadDataSet()
        {
            if (Directory.Exists(this.SelectedFolderPath) == false)
            {
                return;
            }

            try
            {
                this.LoadedData = await this._parsingService.LoadDataSet(this.SelectedFolderPath);
                this.TryClose(true);
            }
            catch (Exception e)
            {
                this._messageManager.ShowMessageBox(e.Message, "Error while loading data set", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectFolder()
        {
            var result = this._messageManager.ShowSelectFolderDialog();

            if (result.result.GetValueOrDefault() == false)
                return;

            this.SelectedFolderPath = result.result.GetValueOrDefault() ? result.selectedPath : string.Empty;

            try
            {
                SafeDirectoryHelper.EnsureS1SAFEDirectory(this.SelectedFolderPath);
                this.IsValidFolder = true;
            }
            catch (Exception e)
            {
                this.SelectedFolderErrorMessage = e.Message;
                this.IsValidFolder = false;
            }
        }
    }
}