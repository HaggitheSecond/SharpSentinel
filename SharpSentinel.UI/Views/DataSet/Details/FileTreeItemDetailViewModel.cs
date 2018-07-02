using System.Diagnostics;
using System.IO;
using MahApps.Metro.IconPacks;
using SharpSentinel.UI.Common;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class FileTreeItemDetailViewModel : TreeItemDetail
    {
        private FileInfo _file;

        public FileInfo File
        {
            get { return this._file; }
            set { this.Set(ref this._file, value); }
        }

        public FluidCommand OpenLocationCommand { get; }
        public FluidCommand OpenFileCommand { get; }

        public FileTreeItemDetailViewModel()
        : base(1, PackIconModernKind.Page)
        {
            this.OpenLocationCommand = FluidCommand.Sync(() => Process.Start(this.File.DirectoryName));
            this.OpenFileCommand = FluidCommand.Sync(() => Process.Start(this.File.FullName));
        }

        public void Initialize(FileInfo file)
        {
            this.File = file;
        }

        public override string GetDisplayName()
        {
            return "File info";
        }
    }
}