using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
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
    public class MainViewModel : ViewModelBase
    {
        private object? _contentTitle;
        private object? _subContent;
        private bool _isCodeOpened;
        private WorkspaceInfoModel? _workspaceInfoCurrent;
        private WorkspaceItemModel? _workspaceItemModel;
        private readonly DataService _dataService;

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;
            WorkspaceInfoCollection = new ObservableCollection<WorkspaceInfoModel>();

            UpdateMainContent();
            UpdateLeftContent();
        }

        public WorkspaceItemModel? WorkspaceItemCurrent
        {
            get => _workspaceItemModel;
            set => SetProperty(ref _workspaceItemModel, value);
        }

        public WorkspaceInfoModel? WorkspaceInfoCurrent
        {
            get => _workspaceInfoCurrent;
            set => SetProperty(ref _workspaceInfoCurrent, value);
        }

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
            object? parameter = _dataService.GetWorkspaceUrl(WorkspaceInfoCurrent, WorkspaceItemCurrent);
            if (parameter == null)
            {
                return;
            }
            ControlCommands.OpenLink.Execute(parameter);
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
            //// 注册接收 ThemeChangedMessage
            //StrongReferenceMessenger.Default.Register()
            WeakReferenceMessenger.Default.Register<MainViewModel,string>(this, MessageToken.LoadShowContent, (obj, msg) =>
            {
                if (SubContent is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                SubContent = obj;
            });
            //ViewModelLocator.Instance.Main.MessengerInstance?.Register<MainViewModel, OnUpdateMainContentMessage>(this, MessageToken.LoadShowContent, obj =>
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
            //clear status
            WeakReferenceMessenger.Default.Register<MainViewModel, string>(this, MessageToken.ClearLeftSelected, (obj, msg) =>
            {
                WorkspaceItemCurrent = null;
                foreach (var item in WorkspaceInfoCollection)
                {
                    item.SelectedIndex = -1;
                }
            });

            WeakReferenceMessenger.Default.Register<MainViewModel, string>(this, MessageToken.LangUpdated, (obj, msg) =>
            {
                if (WorkspaceItemCurrent == null) return;
                ContentTitle = LangProvider.GetLang(WorkspaceItemCurrent.Name);
            });

            //load items
            WorkspaceInfoCollection = _dataService.GetWorkspaceDataList();
            //Task.Run(() =>
            //{
            //    foreach (var item in _dataService.GetWorkspaceInfo())
            //    {
            //        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //        {
            //            WorkspaceInfoCollection.Add(item);
            //        }), DispatcherPriority.ApplicationIdle);
            //    }
            //});
        }

        private void SwitchWorkspace(SelectionChangedEventArgs? e)
        {
            if (e == null || e.AddedItems.Count == 0)
            {
                return;
            }
            if (e.AddedItems[0] is WorkspaceItemModel item)
            {
                System.Windows.Controls.TabControl? tabControl = e.Source as System.Windows.Controls.TabControl;
                switch (tabControl?.SelectedIndex)
                {
                    case 0:
                        if (Equals(WorkspaceItemCurrent, item))
                        {
                            return;
                        }
                        SwitchWorkspace(item);
                        break;
                    case 1:
                        break;
                    default:
                        return;
                }
                
            }
        }

        public void SwitchWorkspace(WorkspaceItemModel item)
        {
            if (WorkspaceInfoCollection.Count > 1)
            {
                Growl.Ask("已经加载了模板，是否切换？", isConfirmed =>
                {
                    if (isConfirmed)
                    {
                        WorkspaceInfoModel? workspace = _dataService.LoadProjectTemplate(item);
                        if (workspace != null)
                        {
                            WorkspaceInfoCollection.RemoveAt(1);
                            WorkspaceItemCurrent = item;
                            ContentTitle = XStudio.App.Properties.Langs.LangProvider.GetLang(item.Name);
                            WorkspaceInfoCollection.Add(workspace);
                        }
                    }
                    return true;
                }, "");
            }
            else
            {
                WorkspaceInfoModel? workspace = _dataService.LoadProjectTemplate(item);
                if (workspace != null)
                {
                    WorkspaceItemCurrent = item;
                    ContentTitle = XStudio.App.Properties.Langs.LangProvider.GetLang(item.Name);
                    WorkspaceInfoCollection.Add(workspace);
                }
            }

            //var obj = AssemblyHelper.ResolveByKey(item.TargetCtlName);
            //var ctl = obj ?? AssemblyHelper.CreateInternalInstance($"UserControl.{item.TargetCtlName}");
            //WeakReferenceMessenger.Default.Send(ctl, MessageToken.FullSwitch);
            //WeakReferenceMessenger.Default.Send(ctl, MessageToken.LoadShowContent);
        }

        private void OpenPracticalDemo()
        {
            //WeakReferenceMessenger.Default.Send<object>(null, MessageToken.ClearLeftSelected);
            //WeakReferenceMessenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.PracticalDemo}"), MessageToken.LoadShowContent);
            //WeakReferenceMessenger.Default.Send(true, MessageToken.FullSwitch);
        }
    }
}
