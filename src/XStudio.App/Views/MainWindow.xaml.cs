using System;
using System.Windows;
using XStudio.App.Common;
using XStudio.App.Service;

namespace XStudio.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DataService _helloWorldService;

    public MainWindow(DataService helloWorldService)
    {
        _helloWorldService = helloWorldService;
        InitializeComponent();
    }

    protected override void OnContentRendered(EventArgs e)
    {
        //HelloLabel.Content = _helloWorldService.SayHello();
    }
}
