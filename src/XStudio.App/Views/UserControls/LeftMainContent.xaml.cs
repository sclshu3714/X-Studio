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
            if (e.AddedItems[0] is WorkspaceInfoModel demoInfo)
            {
                ViewModelLocator.Instance.Main.WorkspaceInfoCurrent = demoInfo;
                var selectedIndex = demoInfo.SelectedIndex;
                demoInfo.SelectedIndex = -1;
                demoInfo.SelectedIndex = selectedIndex;

                FilterItems();
                GroupItems(sender as TabControl, demoInfo);
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
            if (string.IsNullOrEmpty(_searchKey))
            {
                foreach (var item in ViewModelLocator.Instance.Main.WorkspaceInfoCurrent.DataList)
                {
                    //item.IsVisible = true;
                    //item.QueriesText = string.Empty;
                }
            }
            else
            {
                var key = _searchKey.ToLower();
                foreach (var item in ViewModelLocator.Instance.Main.WorkspaceInfoCurrent.DataList)
                {
                    if (item.Name.ToLower().Contains(key))
                    {
                        //item.IsVisible = true;
                        //item.QueriesText = _searchKey;
                    }
                    //else if (item.TargetCtlName.Replace("DemoCtl", "").ToLower().Contains(key))
                    //{
                    //    item.IsVisible = true;
                    //    item.QueriesText = _searchKey;
                    //}
                    //else
                    //{
                    //    var name = Properties.Langs.LangProvider.GetLang(item.Name);
                    //    if (!string.IsNullOrEmpty(name) && name.ToLower().Contains(key))
                    //    {
                    //        item.IsVisible = true;
                    //        item.QueriesText = _searchKey;
                    //    }
                    //    else
                    //    {
                    //        item.IsVisible = false;
                    //        item.QueriesText = string.Empty;
                    //    }
                    //}
                }
            }
        }

        private void GroupItems(TabControl? tabControl, WorkspaceInfoModel demoInfo)
        {
            var listBox = VisualHelper.GetChild<ListBox>(tabControl);
            if (listBox == null) return;
            listBox.Items.GroupDescriptions?.Clear();

            if (demoInfo.IsGroupEnabled)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    listBox.Items.GroupDescriptions?.Add(new PropertyGroupDescription("GroupName"));
                }), DispatcherPriority.Background);
            }
        }
    }
}
