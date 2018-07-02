using MahApps.Metro.IconPacks;
using SharpSentinel.Parser.Data.ManifestObjects;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class MetaDataTreeItemDetailViewModel : TreeItemDetail
    {
        private MetaData _metaData;

        public MetaData MetaData
        {
            get { return this._metaData; }
            set { this.Set(ref this._metaData, value); }
        }

        public MetaDataTreeItemDetailViewModel() 
            : base(10, PackIconModernKind.Database)
        {
        }

        public void Initialize(MetaData metaData)
        {
            this.MetaData = metaData;
        }

        public override string GetDisplayName()
        {
            return "Meta data";
        }
    }
}