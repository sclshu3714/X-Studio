using HandyControl.Data;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using XStudio.App.ViewModel.Main;
using XStudio.App.ViewModel;
using XStudio.App.Helper;
using System.Reflection;
using HandyControl.Controls;
using XStudio.App.Service;

namespace XStudio.App.Views.UserControls
{
    /// <summary>
    /// LeftMainContent.xaml 的交互逻辑
    /// </summary>
    public partial class LeftMainContent : UserControl
    {
        private string _searchKey = string.Empty;
        public LeftMainContent()
        {
            InitializeComponent();
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is WorkspaceInfoModel workspaceInfo)
            {
                ViewModelLocator.Instance.Main.WorkspaceInfoCurrent = workspaceInfo;
                var selectedIndex = workspaceInfo.SelectedIndex;
                workspaceInfo.SelectedIndex = -1;
                workspaceInfo.SelectedIndex = selectedIndex;

                FilterItems();
                GroupItems(sender as System.Windows.Controls.TabControl, workspaceInfo);
            }
            else if (e.AddedItems[0] is WorkspaceItemModel workspaceItem)
            {
                if (ViewModelLocator.Instance.Main.WorkspaceInfoCollection.Count != 1)
                {
                    return;
                }
                ViewModelLocator.Instance.Main.SwitchWorkspace(workspaceItem);
            }
        }

        private void ButtonAscending_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton button && button.Tag is ItemsControl itemsControl)
            {
                if (button.IsChecked == true)
                {
                    itemsControl.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                else
                {
                    itemsControl.Items.SortDescriptions.Clear();
                }
            }
        }

        private void SearchBar_OnSearchStarted(object sender, FunctionEventArgs<string> e)
        {
            _searchKey = e.Info;
            FilterItems();
        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(_searchKey) && ViewModelLocator.Instance.Main.WorkspaceInfoCurrent != null)
            {
                foreach (var item in ViewModelLocator.Instance.Main.WorkspaceInfoCurrent.DataList)
                {
                    item.IsVisible = true;
                    item.QueriesText = string.Empty;
                }
            }
            else if (ViewModelLocator.Instance.Main.WorkspaceInfoCurrent != null)
            {
                var key = _searchKey.ToLower();
                foreach (var item in ViewModelLocator.Instance.Main.WorkspaceInfoCurrent.DataList)
                {
                    if (item.Name.ToLower().Contains(key))
                    {
                        item.IsVisible = true;
                        item.QueriesText = _searchKey;
                    }
                    else if (item.TargetCtlName.Replace("Ctl", "").ToLower().Contains(key))
                    {
                        item.IsVisible = true;
                        item.QueriesText = _searchKey;
                    }
                    else
                    {
                        var name = Properties.Langs.LangProvider.GetLang(item.Name);
                        if (!string.IsNullOrEmpty(name) && name.ToLower().Contains(key))
                        {
                            item.IsVisible = true;
                            item.QueriesText = _searchKey;
                        }
                        else
                        {
                            item.IsVisible = false;
                            item.QueriesText = string.Empty;
                        }
                    }
                }
            }
        }

        private void GroupItems(System.Windows.Controls.TabControl? tabControl, WorkspaceInfoModel demoInfo)
        {
            if (tabControl == null) return;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var listBox = VisualHelper.GetChild<ListBox>(tabControl);
                if (listBox == null) return;
                listBox.Items.GroupDescriptions?.Clear();

                if (demoInfo.IsGroupEnabled)
                {
                    listBox.Items.GroupDescriptions?.Add(new PropertyGroupDescription("GroupName"));
                }
            }), DispatcherPriority.Background);
        }
    }
}
