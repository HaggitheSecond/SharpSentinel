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
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;

namespace SharpSentinel.UI.Views.DataSet
{
    /// <summary>
    /// Interaction logic for FileTreeView.xaml
    /// </summary>
    public partial class FileTreeView
    {
        public FileTreeViewModel ViewModel => this.DataContext as FileTreeViewModel;

        public FileTreeView()
        {
            this.InitializeComponent();

            this.DataContextChanged += (sender, args) =>
            {
                this.ViewModel.PropertyChanged += (sende2r, args2) =>
                {
                    if (args2.PropertyName == nameof(this.ViewModel.MainItem) && this.ViewModel.MainItem != null)
                        this.GenerateTree();

                };
            };
        }

        public void GenerateTree()
        {
            var mainItem = this.GenerateTreeItem(this.ViewModel.MainItem);
            mainItem.IsExpanded = true;
            this.TreeView.Items.Add(mainItem);
        }

        private TreeViewItem GenerateTreeItem(TreeItem item)
        {
            var treeItem = new TreeViewItem
            {
                Header = this.GenerateHeader(item),
                ContextMenu = new ContextMenu
                {
                    Items =
                    {
                        new MenuItem
                        {
                            Command = item.OpenFileCommand,
                            Header = new StackPanel
                            {
                                Orientation = Orientation.Horizontal,
                                Children =
                                {
                                    new PackIconModern
                                    {
                                        Kind = item is DirectoryTreeItem ? PackIconModernKind.FolderOpen : PackIconModernKind.Fullscreen,
                                        Margin = new Thickness(0, 0, 5, 0)
                                    },
                                    new TextBlock
                                    {
                                        Text = "Open"
                                    }
                                }
                            }
                        }
                    }
                },
                ToolTip = new ToolTip
                {
                    Content = string.IsNullOrWhiteSpace(item.ToolTip) ? item.DisplayText : item.ToolTip
                }
            };

            if (!(item is DirectoryTreeItem directoryItem))
                return treeItem;

            foreach (var currentChild in directoryItem.Children)
            {
                treeItem.Items.Add(this.GenerateTreeItem(currentChild));
            }

            return treeItem;
        }

        private StackPanel GenerateHeader(TreeItem item)
        {
            var textBlock = new TextBlock();
            var textBinding = new Binding("DisplayText") { Source = item };
            BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, textBinding);

            var packIconModern = new PackIconModern
            {
                Margin = new Thickness(0, 0, 5, 0)
            };
            var iconBinding = new Binding("Icon") { Source = item };
            BindingOperations.SetBinding(packIconModern, PackIconModern.KindProperty, iconBinding);

            return new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    packIconModern,
                    textBlock
                }
            };
        }
    }
}
