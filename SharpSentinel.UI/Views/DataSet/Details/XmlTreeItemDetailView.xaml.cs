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
    /// Interaction logic for XmlTreeItemDetailView.xaml
    /// </summary>
    public partial class XmlTreeItemDetailView : UserControl
    {
        public XmlTreeItemDetailView()
        {
            this.InitializeComponent();

            // This should be done in xaml but no idea how to properly work with large xml files in richtextbox in the first place
            this.RichTextBox.Document.PageWidth = 2000;
        }
    }
}
