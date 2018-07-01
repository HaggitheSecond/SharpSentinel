using System.IO;
using System.Linq;
using Caliburn.Micro;
using MahApps.Metro.IconPacks;
using SharpSentinel.Parser.Data;

namespace SharpSentinel.UI.Views.DataSet
{
    public class FileTreeViewModel : Screen
    {
        private FileTreeItem _mainItem;

        public FileTreeItem MainItem
        {
            get { return this._mainItem; }
            set { this.Set(ref this._mainItem, value); }
        }

        public FileTreeViewModel()
        {

        }

        public void Initialize(BaseData baseData)
        {
            if (!(baseData is S1Data s1Data))
                return;

            var allDirectories = new DirectoryInfo(baseData.BaseDirectory.FullName).GetDirectories();

            var annotationDirectory = allDirectories.First(f => f.Name == "annotation");
            var measurementDirectory = allDirectories.First(f => f.Name == "measurement");
            var previewDirectory = allDirectories.First(f => f.Name == "preview");
            var supportDirectory = allDirectories.First(f => f.Name == "support");

            var calibrationDirectory = annotationDirectory.GetDirectories().First(f => f.Name == "calibration");

            var annotationTreeItem = new FileTreeItem(annotationDirectory)
            {
                Children = new BindableCollection<FileTreeItem>()
            };
            var calibrationTreeItem = new FileTreeItem(calibrationDirectory)
            {
                Children = new BindableCollection<FileTreeItem>()
            };

            calibrationTreeItem.Children.AddRange(s1Data.MeasurementDataUnits.Select(f => f.CalibriationAnnotation).Select(f => new FileTreeItem(f.File)
            {
                Data = f
            }));
            calibrationTreeItem.Children.AddRange(s1Data.MeasurementDataUnits.Select(f => f.NoiseAnnotation).Select(f => new FileTreeItem(f.File)
            {
                Data = f
            }));

            annotationTreeItem.Children.Add(calibrationTreeItem);
            annotationTreeItem.Children.AddRange(s1Data.MeasurementDataUnits
                .Select(f => f.ProductAnnotation).Select(f => new FileTreeItem(f.File)
                {
                    Data = f
                }));

            var mainItem = new FileTreeItem(baseData.BaseDirectory)
            {
                DisplayText = baseData.BaseDirectory.Name,
                Children = new BindableCollection<FileTreeItem>
                {
                    annotationTreeItem,
                    new FileTreeItem(measurementDirectory)
                    {
                        Children = new BindableCollection<FileTreeItem>(s1Data.MeasurementDataUnits.Select(f => new FileTreeItem(f.File)
                        {
                            Data = f
                        }))
                    },
                    new FileTreeItem(previewDirectory),
                    new FileTreeItem(supportDirectory),
                    new FileTreeItem(baseData.Manifest.File)
                    {
                        Data = baseData.Manifest,
                        DisplayText = baseData.Manifest.File.Name
                    },
                }
            };

            this.MainItem = mainItem;
        }
    }

    public class FileTreeItem : PropertyChangedBase
    {
        private BindableCollection<FileTreeItem> _children;
        private string _displayText;
        private PackIconModernKind _icon;
        private DirectoryInfo _directoryInfo;
        private FileInfo _fileInfo;
        private FileTreeItemType _itemType;
        private object _data;

        public BindableCollection<FileTreeItem> Children
        {
            get { return this._children; }
            set { this.Set(ref this._children, value); }
        }

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

        public DirectoryInfo DirectoryInfo
        {
            get { return this._directoryInfo; }
            set { this.Set(ref this._directoryInfo, value); }
        }

        public FileInfo FileInfo
        {
            get { return this._fileInfo; }
            set { this.Set(ref this._fileInfo, value); }
        }

        public FileTreeItemType ItemType
        {
            get { return this._itemType; }
            set { this.Set(ref this._itemType, value); }
        }

        public object Data
        {
            get { return this._data; }
            set { this.Set(ref this._data, value); }
        }

        private FileTreeItem()
        {
            this.Children = new BindableCollection<FileTreeItem>();
        }

        public FileTreeItem(FileInfo fileInfo)
        : this()
        {
            this.FileInfo = fileInfo;
            this.ItemType = FileTreeItemType.File;

            this.DisplayText = fileInfo.Name;

            switch (fileInfo.Extension)
            {
                case ".xml":
                    this.Icon = PackIconModernKind.PageXml;
                    break;
                case ".tiff":
                    this.Icon = PackIconModernKind.Map;
                    break;
                default:
                    this.Icon = PackIconModernKind.Page;
                    break;
            }
        }

        public FileTreeItem(DirectoryInfo directoryInfo)
        : this()
        {
            this.DirectoryInfo = directoryInfo;
            this.ItemType = FileTreeItemType.Directory;
            this.Icon = PackIconModernKind.Folder;

            this.DisplayText = directoryInfo.Name;
        }
    }

    public enum FileTreeItemType
    {
        File,
        Directory
    }
}