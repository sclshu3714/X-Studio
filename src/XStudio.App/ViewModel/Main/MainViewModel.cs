using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using XStudio.App.Helper;
using XStudio.App.Models;
using XStudio.App.Models.Data;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Main
{
    public class MainViewModel : ViewModelDataBase<DataModel>
    {
        private object? _contentTitle;
        private object? _subContent;
        private bool _isCodeOpened;

        private readonly DataService _dataService;

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
            WorkspaceInfoCollection = new ObservableCollection<WorkspaceInfoModel>();

            UpdateMainContent();
            UpdateLeftContent();
        }

        public WorkspaceItemModel? WorkspaceItemCurrent { get; private set; }

        public WorkspaceInfoModel? WorkspaceInfoCurrent { get; set; }

        public object? SubContent
        {
            get => _subContent;
            set => SetProperty(ref _subContent, value);
        }

        public object? ContentTitle
        {
            get => _contentTitle;
            set => SetProperty(ref _contentTitle, value);
        }

        public bool IsCodeOpened
        {
            get => _isCodeOpened;
            set => SetProperty(ref _isCodeOpened, value);
        }

        public ObservableCollection<WorkspaceInfoModel> WorkspaceInfoCollection { get; set; }

        public RelayCommand<SelectionChangedEventArgs> SwitchWorkspaceCmd => new(SwitchWorkspace);

        public RelayCommand OpenPracticalDemoCmd => new(OpenPracticalDemo);

        public RelayCommand GlobalShortcutInfoCmd => new(() => { });

        public RelayCommand GlobalShortcutWarningCmd => new(() => { });

        public RelayCommand OpenDocCmd => new(() =>
        {
            if (WorkspaceItemCurrent is null)
            {
                return;
            }

            // ControlCommands.OpenLink.Execute(_dataService.GetWorkspaceUrl(WorkspaceInfoCurrent, WorkspaceItemCurrent));
        });

        public RelayCommand OpenCodeCmd => new(() =>
        {
            if (WorkspaceItemCurrent is null)
            {
                return;
            }

            IsCodeOpened = !IsCodeOpened;
        });

        private void UpdateMainContent()
        {
            //Messenger.Default.Register<object>(this, MessageToken.LoadShowContent, obj =>
            //{
            //    if (SubContent is IDisposable disposable)
            //    {
            //        disposable.Dispose();
            //    }
            //    SubContent = obj;
            //}, true);
        }

        private void UpdateLeftContent()
        {
            ////clear status
            //Messenger.Default.Register<object>(this, MessageToken.ClearLeftSelected, obj =>
            //{
            //    WorkspaceItemCurrent = null;
            //    foreach (var item in WorkspaceInfoCollection)
            //    {
            //        item.SelectedIndex = -1;
            //    }
            //});

            //Messenger.Default.Register<object>(this, MessageToken.LangUpdated, obj =>
            //{
            //    if (WorkspaceItemCurrent == null) return;
            //    ContentTitle = LangProvider.GetLang(WorkspaceItemCurrent.Name);
            //});

            //load items
            WorkspaceInfoCollection = new ObservableCollection<WorkspaceInfoModel>();
            Task.Run(() =>
            {
                DataList = _dataService.GetWorkspaceDataList();
                foreach (var item in _dataService.GetWorkspaceInfo())
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        WorkspaceInfoCollection.Add(item);
                    }), DispatcherPriority.ApplicationIdle);
                }
            });
        }

        private void SwitchWorkspace(SelectionChangedEventArgs? e)
        {
            if (e.AddedItems.Count == 0) return;
            if (e.AddedItems[0] is WorkspaceItemModel item)
            {
                if (Equals(WorkspaceItemCurrent, item)) return;
                SwitchWorkspace(item);
            }
        }

        private void SwitchWorkspace(WorkspaceItemModel item)
        {
            WorkspaceItemCurrent = item;
            ContentTitle = LangProvider.GetLang(item.Name);
            var obj = AssemblyHelper.ResolveByKey(item.TargetCtlName);
            var ctl = obj ?? AssemblyHelper.CreateInternalInstance($"UserControl.{item.TargetCtlName}");
            //Messenger.Default.Send(ctl is IFull, MessageToken.FullSwitch);
            //Messenger.Default.Send(ctl, MessageToken.LoadShowContent);
        }

        private void OpenPracticalDemo()
        {
            //Messenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
            //Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.PracticalDemo}"), MessageToken.LoadShowContent);
            //Messenger.Default.Send(true, MessageToken.FullSwitch);
        }
    }
}
