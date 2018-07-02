using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    public class NoDetailsTreeItemDetailViewModel : TreeItemDetail
    {
        public NoDetailsTreeItemDetailViewModel() 
            : base(0, PackIconModernKind.PageQuestion)
        {
        }

        public override string GetDisplayName()
        {
            return "No details";
        }
    }
}