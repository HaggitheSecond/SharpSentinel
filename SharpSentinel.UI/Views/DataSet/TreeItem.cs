using System.Diagnostics;
using System.IO;
using Caliburn.Micro;
using MahApps.Metro.IconPacks;
using SharpSentinel.Parser.Data.Interfaces;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.UI.Common;
using SharpSentinel.UI.Views.DataSet.Details;

namespace SharpSentinel.UI.Views.DataSet
{
    public abstract class TreeItem : PropertyChangedBase
    {
        private string _displayText;
        private PackIconModernKind _icon;
        private string _toolTip;
        private BindableCollection<TreeItemDetail> _detailPages;

        public string DisplayText
        {
            get { return this._displayText; }
            set { this.Set(ref this._displayText, value); }
        }

        public PackIconModernKind Icon
        {
            get { return this._icon; }
            set { this.Set(ref this._icon, value); }
        }

        public string ToolTip
        {
            get { return this._toolTip; }
            set { this.Set(ref this._toolTip, value); }
        }

        public BindableCollection<TreeItemDetail> DetailPages
        {
            get { return this._detailPages; }
            set { this.Set(ref this._detailPages, value); }
        }

        public FluidCommand OpenFileCommand { get; }

        protected TreeItem(string displayText, FluidCommand openCommand, PackIconModernKind icon = PackIconModernKind.Page, string toolTip = "")
        {
            this.DisplayText = displayText;
            this.Icon = icon;
            this.OpenFileCommand = openCommand;
            this.ToolTip = toolTip;
            this.DetailPages = new BindableCollection<TreeItemDetail>();
        }
    }

    public class FileTreeItem : TreeItem
    {
        private FileInfo _fileInfo;
        private object _data;

        public FileInfo FileInfo
        {
            get { return this._fileInfo; }
            set { this.Set(ref this._fileInfo, value); }
        }

        public object Data
        {
            get { return this._data; }
            set { this.Set(ref this._data, value); }
        }

        public FileTreeItem(FileInfo fileInfo, object data = null, string toolTip = "")
            : base(fileInfo.Name, FluidCommand.Sync(() => Process.Start(fileInfo.FullName)), toolTip: toolTip)
        {
            this.FileInfo = fileInfo;

            this.SetIconByFileExtension(fileInfo.Extension);

            if (data == null)
                return;

            this.Data = data;

            if (this.Data is IFile file)
            {
                var fileViewModel = IoC.Get<FileTreeItemDetailViewModel>();
                fileViewModel.Initialize(file.File);
                this.DetailPages.Add(fileViewModel);
            }

            if (this.Data is IXmlFile xmlFile)
            {
                var xmlViewModel = IoC.Get<XmlTreeItemDetailViewModel>();
                xmlViewModel.Intialize(xmlFile.RawXml, xmlFile.File.Name);
                this.DetailPages.Add(xmlViewModel);
            }
        }

        private void SetIconByFileExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".xml":
                    this.Icon = PackIconModernKind.PageXml;
                    break;
                case ".tiff":
                    this.Icon = PackIconModernKind.Database;
                    break;
                case ".safe":
                    this.Icon = PackIconModernKind.PageXml;
                    break;
                case ".pdf":
                    this.Icon = PackIconModernKind.PageFilePdfTag;
                    break;
                default:
                    this.Icon = PackIconModernKind.Page;
                    break;
            }
        }
    }

    public class DirectoryTreeItem : TreeItem
    {
        private BindableCollection<TreeItem> _children;
        private DirectoryInfo _directoryInfo;

        public BindableCollection<TreeItem> Children
        {
            get { return this._children; }
            set { this.Set(ref this._children, value); }
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return this._directoryInfo; }
            set { this.Set(ref this._directoryInfo, value); }
        }

        public DirectoryTreeItem(DirectoryInfo directoryInfo)
            : base(directoryInfo.Name, FluidCommand.Sync(() => Process.Start(directoryInfo.FullName)), PackIconModernKind.Folder)
        {
            this.Children = new BindableCollection<TreeItem>();
            this.DirectoryInfo = directoryInfo;
        }
    }
}