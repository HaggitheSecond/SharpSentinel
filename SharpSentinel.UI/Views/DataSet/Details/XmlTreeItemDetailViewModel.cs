using System.Linq;
using MahApps.Metro.IconPacks;
using SharpSentinel.UI.Extensions;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class XmlTreeItemDetailViewModel : TreeItemDetail
    {
        private string _rawXml;
        private bool _xmlTooLarge;

        public string RawXml
        {
            get { return this._rawXml; }
            set { this.Set(ref this._rawXml, value); }
        }
        
        public bool XmlTooLarge
        {
            get { return this._xmlTooLarge; }
            set { this.Set(ref this._xmlTooLarge, value); }
        }

        public XmlTreeItemDetailViewModel()
        : base(2, PackIconModernKind.PageXml)
        {

        }

        public void Intialize(string rawXml, string fileName)
        {
            if (rawXml.Length > 30000)
            {
                this.XmlTooLarge = true;
                this.Icon = PackIconModernKind.Alert;
            }

            this.RawXml = rawXml.FormatXml();
        }


        public override string GetDisplayName()
        {
            return "Raw Xml";
        }
    }
}