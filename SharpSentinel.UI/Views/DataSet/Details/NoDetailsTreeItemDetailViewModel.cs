using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class NoDetailsTreeItemDetailViewModel : TreeItemDetail
    {
        public NoDetailsTreeItemDetailViewModel() 
            : base(2, PackIconModernKind.PageQuestion)
        {
        }

        public override string GetDisplayName()
        {
            return "No details";
        }
    }
}