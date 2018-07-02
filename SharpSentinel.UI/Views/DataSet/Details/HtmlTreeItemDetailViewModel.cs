using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class HtmlTreeItemDetailViewModel : TreeItemDetail
    {
        private string _htmlText;

        public string HtmlText
        {
            get { return this._htmlText; }
            set { this.Set(ref this._htmlText, value); }
        }

        public HtmlTreeItemDetailViewModel() 
            : base(3, PackIconModernKind.PageCode)
        {

        }

        public void Initialize(string htmlText)
        {
            this.HtmlText = htmlText;
        }

        public override string GetDisplayName()
        {
            return "HTML";
        }
    }
}