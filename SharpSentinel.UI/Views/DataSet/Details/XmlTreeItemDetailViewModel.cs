using System.Linq;
using MahApps.Metro.IconPacks;
using SharpSentinel.UI.Common;
using SharpSentinel.UI.Extensions;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class XmlTreeItemDetailViewModel : TreeItemDetail
    {
        private string _rawXml;
        private string _entireRawXml;
        private bool _isXmlTooLarge;
        private bool _isEntireXmlLoaded;

        public string RawXml
        {
            get { return this._rawXml; }
            set { this.Set(ref this._rawXml, value); }
        }

        public bool IsXmlTooLarge
        {
            get { return this._isXmlTooLarge; }
            set { this.Set(ref this._isXmlTooLarge, value); }
        }

        public bool IsEntireXmlLoaded
        {
            get { return this._isEntireXmlLoaded; }
            set { this.Set(ref this._isEntireXmlLoaded, value); }
        }

        public FluidCommand LoadEntireFileCommand { get; }

        public XmlTreeItemDetailViewModel()
            : base(2, PackIconModernKind.PageXml)
        {
            this.LoadEntireFileCommand = FluidCommand.Sync(this.LoadEntireFile);
        }

        private const int TooLargeLenght = 30000;

        public void Intialize(string rawXml)
        {
            this._entireRawXml = rawXml.FormatXml();

            if (rawXml.Length > TooLargeLenght)
            {
                this.IsXmlTooLarge = true;
                this.Icon = PackIconModernKind.Alert;
                this.RawXml = this._entireRawXml.Substring(0, TooLargeLenght);
                this.IsEntireXmlLoaded = false;
            }
            else
            {
                this.RawXml = this._entireRawXml;
                this.IsEntireXmlLoaded = true;
            }

        }

        public override void Deactivate()
        {
            if (this._entireRawXml.Length < TooLargeLenght)
                return;


            this.RawXml = this._entireRawXml.Substring(0, TooLargeLenght);
            this.IsEntireXmlLoaded = false;
        }

        private void LoadEntireFile()
        {
            this.RawXml = this._entireRawXml;
            this.IsEntireXmlLoaded = true;
        }


        public override string GetDisplayName()
        {
            return "Raw Xml";
        }
    }
}