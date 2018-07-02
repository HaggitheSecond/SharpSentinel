using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpSentinel.UI.Views.DataSet.Details
{
    /// <summary>
    /// Interaction logic for HtmlTreeItemDetailView.xaml
    /// </summary>
    public partial class HtmlTreeItemDetailView : UserControl
    {
        public HtmlTreeItemDetailViewModel ViewModel => this.DataContext as HtmlTreeItemDetailViewModel;

        public HtmlTreeItemDetailView()
        {
            this.InitializeComponent();

            this.DataContextChanged += (sender, args) =>
            {
                if (this.ViewModel != null)
                    this.InsertHtml();
            };
        }

        private void InsertHtml()
        {
            if (string.IsNullOrWhiteSpace(this.ViewModel.HtmlText))
                return;

            this.WebBrowser.NavigateToString(this.ViewModel.HtmlText);
        }
    }
}
