using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Windows.Input;
using XStudio.App.Common;
using XStudio.App.Models.Data;
using XStudio.App.Service;
using XStudio.App.ViewModel;
using XStudio.App.Views.UserControls;

namespace XStudio.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : HandyControl.Controls.Window
{
    private readonly DataService _helloWorldService;

    public MainWindow(DataService helloWorldService)
    {
        _helloWorldService = helloWorldService;
        InitializeComponent();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        base.OnContentRendered(e);

        DataContext = ViewModelLocator.Instance.Main;
        NonClientAreaContent = new NonClientAreaContent();
        ControlMain.Content = new MainWindowContent();

        GlobalShortcut.Init(new List<KeyBinding>
        {
            new(ViewModelLocator.Instance.Main.GlobalShortcutInfoCmd, Key.I, ModifierKeys.Control | ModifierKeys.Alt),
            new(ViewModelLocator.Instance.Main.GlobalShortcutWarningCmd, Key.E, ModifierKeys.Control | ModifierKeys.Alt),
            new(ViewModelLocator.Instance.Main.OpenDocCmd, Key.F1, ModifierKeys.None),
            new(ViewModelLocator.Instance.Main.OpenCodeCmd, Key.F12, ModifierKeys.None)
        });
        Dialog.SetToken(this, MessageToken.MainWindow);
        WindowAttach.SetIgnoreAltF4(this, true);

       // Messenger.Default.Send(true, MessageToken.FullSwitch);
        //Messenger.Default.Send(AssemblyHelper.CreateInternalInstance($"UserControl.{MessageToken.PracticalDemo}"), MessageToken.LoadShowContent);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (GlobalData.NotifyIconIsShow)
        {
            //HandyControl.Controls.MessageBox.Info(HandyControl.Properties.Langs.Lang.AppClosingTip, HandyControl.Properties.Langs.Lang.Tip);
            Hide();
            e.Cancel = true;
        }
        else
        {
            base.OnClosing(e);
        }
    }
}
