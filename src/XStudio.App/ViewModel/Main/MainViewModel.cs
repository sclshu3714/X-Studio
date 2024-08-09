﻿using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Properties.Langs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using XStudio.App.Models;
using XStudio.App.Service;

namespace XStudio.App.ViewModel.Main
{
    public class MainViewModel : ViewModelBase<DataModel>
    {
        private readonly DataService _dataService;

        private object? _contentTitle;
        private object? _subContent;

        public MainViewModel(DataService dataService)
        {
            _dataService = dataService;

            //UpdateMainContent();
            //UpdateLeftContent();
        }

        //public DemoItemModel? DemoItemCurrent { get; private set; }

        //public DemoInfoModel? DemoInfoCurrent { get; set; }

        //public object? SubContent
        //{
        //    get => _subContent;
        //    set => SetProperty(ref _subContent, value);
        //}

        //public object? ContentTitle
        //{
        //    get => _contentTitle;
        //    set => SetProperty(ref _contentTitle, value);
        //}

        //public ObservableCollection<DemoInfoModel> DemoInfoCollection { get; set; } = [];

        //private void UpdateMainContent()
        //{
        //    WeakReferenceMessenger.Default.Register<DemoItemModel, string>(
        //        recipient: this,
        //        token: MessageToken.SwitchDemo,
        //        handler: (_, message) => SwitchDemo(message)
        //    );
        //}

        //private void UpdateLeftContent()
        //{
        //    //clear status
        //    WeakReferenceMessenger.Default.Register<object, string>(this, MessageToken.ClearLeftSelected, (_, _) =>
        //    {
        //        DemoItemCurrent = null;
        //        foreach (var item in DemoInfoCollection)
        //        {
        //            item.SelectedIndex = -1;
        //        }
        //    });

        //    WeakReferenceMessenger.Default.Register<object, string>(this, MessageToken.LangUpdated, (_, _) =>
        //    {
        //        if (DemoItemCurrent == null)
        //        {
        //            return;
        //        }

        //        ContentTitle = Lang.ResourceManager.GetString(DemoItemCurrent.Name, Lang.Culture);
        //    });

        //    //load items
        //    DemoInfoCollection = [];
        //    foreach (var item in _dataService.GetDemoInfo())
        //    {
        //        Dispatcher.UIThread.InvokeAsync(() => DemoInfoCollection.Add(item));
        //    }
        //    Dispatcher.UIThread.InvokeAsync(() => SwitchDemo(DemoInfoCollection.First().DemoItemList.First()));
        //}

        //private void SwitchDemo(DemoItemModel item)
        //{
        //    if (SubContent is IDisposable disposable)
        //    {
        //        disposable.Dispose();
        //    }

        //    DemoItemCurrent = item;
        //    ContentTitle = Lang.ResourceManager.GetString(item.Name, Lang.Culture);
        //    object? demoControl = AssemblyHelper.ResolveByKey(item.TargetCtlName) ??
        //                          AssemblyHelper.CreateInternalInstance($"UserControl.{item.TargetCtlName}");
        //    SubContent = demoControl;
        //}
    }
}
