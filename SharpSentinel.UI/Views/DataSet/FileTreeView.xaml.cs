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
using Caliburn.Micro;
using MahApps.Metro.IconPacks;
using SharpSentinel.UI.Views.DataSet.Details;

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
                this.ViewModel.PropertyChanged += (propertyChangedSender, propertyChangedArgs) =>
                {
                    if (propertyChangedArgs.PropertyName == nameof(this.ViewModel.MainItem))
                        this.UpdateTree();

                };
            };
        }

        public void UpdateTree()
        {
            if (this.ViewModel.MainItem != null)
            {
                var mainItem = this.GenerateTreeItem(this.ViewModel.MainItem);
                mainItem.IsExpanded = true;
                this.TreeView.Items.Add(mainItem);
            }
            else
            {
                this.TreeView.Items.Clear();
            }
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
                },
                DataContext = item
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

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.DeactivateItems();
            this.TabControl.Items.Clear();

            if (!(e.NewValue is TreeViewItem treeViewItem))
                return;

            if (!(treeViewItem.DataContext is TreeItem treeItem))
                return;

            var orderedItems = treeItem.DetailPages.OrderByDescending(f => f.OrderPriority);
            var details = orderedItems.Select(this.GenerateDetailPage).ToList();
            
            if(details.Count == 0)
                details.Add(this.GenerateDetailPage(new NoDetailsTreeItemDetailViewModel()));

            foreach (var currentDetails in details)
            {
                this.TabControl.Items.Add(currentDetails);
            }

            this.TabControl.SelectedItem = this.TabControl.Items[0];
        }

        private void DeactivateItems()
        {
            foreach (var currentItem in this.TabControl.Items)
            {
                if (currentItem is TabItem tab)
                    if (tab.Tag is TreeItemDetail detail)
                        detail.Deactivate();
            }
        }

        private TabItem GenerateDetailPage(TreeItemDetail treeItemDetail)
        {
            var tabItem = new TabItem
            {
                Header = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Children =
                    {

                        new PackIconModern
                        {
                            Kind = treeItemDetail.Icon,
                            Margin = new Thickness(0, 0, 5, 0)
                        },
                        new TextBlock
                        {
                            Text = treeItemDetail.GetDisplayName()
                        }
                    }
                },
                Tag = treeItemDetail
            };

            var view = ViewLocator.LocateForModel(treeItemDetail, null, null);
            ViewModelBinder.Bind(treeItemDetail, view, null);
            tabItem.Content = view;

            return tabItem;
        }

        /// <summary>
        /// Disable horizontal scrolling when selecting item
        /// </summary>
        private void TreeView_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}
