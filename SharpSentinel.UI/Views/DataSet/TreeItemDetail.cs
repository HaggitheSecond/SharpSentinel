using Caliburn.Micro;
using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet
{
    public abstract class TreeItemDetail : Screen
    {
        private int _orderPriority;
        private PackIconModernKind _icon;

        /// <summary>
        /// The lower the number the higher the priority and the farther left in the tabcontrol
        /// </summary>
        public int OrderPriority
        {
            get { return this._orderPriority; }
            set { this.Set(ref this._orderPriority, value); }
        }

        public PackIconModernKind Icon
        {
            get { return this._icon; }
            set { this.Set(ref this._icon, value); }
        }

        protected TreeItemDetail(int orderPriority, PackIconModernKind? icon = null)
        {
            this.OrderPriority = orderPriority;
            this.Icon = icon.GetValueOrDefault(PackIconModernKind.App);
        }

        public abstract string GetDisplayName();
    }
}