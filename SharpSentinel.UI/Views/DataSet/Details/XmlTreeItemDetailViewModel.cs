using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class XmlTreeItemDetailViewModel : TreeItemDetail
    {
        private string _rawXml;
        private string _fileName;

        public string RawXml
        {
            get { return this._rawXml; }
            set { this.Set(ref this._rawXml, value); }
        }

        public XmlTreeItemDetailViewModel()
        : base(2, PackIconModernKind.PageXml)
        {

        }

        public void Intialize(string rawXml, string fileName)
        {
            this.RawXml = rawXml;
            this._fileName = fileName;
        }


        public override string GetDisplayName()
        {
            return "Raw Xml";
        }
    }
}