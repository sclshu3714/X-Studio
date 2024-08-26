﻿using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using HandyControl.Tools;
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
using XStudio.App.Models.Data;
using XStudio.App.ViewModel;

namespace XStudio.App.Views.UserControls
{
    /// <summary>
    /// MainTabContent.xaml 的交互逻辑
    /// </summary>
    public partial class MainTabContent : UserControl
    {
        private bool _isFull;

        private string _currentDemoKey;

        private bool _drawerCodeUsed;

        //private Dictionary<string, TextEditor> _textEditor;
        public MainTabContent()
        {
            InitializeComponent();

            //Messenger.Default.Register<bool>(this, MessageToken.FullSwitch, FullSwitch);
            //Messenger.Default.Register<SkinType>(this, MessageToken.SkinUpdated, SkinUpdated);
        }

        private void FullSwitch(bool isFull)
        {
            if (_isFull == isFull)
            {
                return;
            }

            _isFull = isFull;

            if (_isFull)
            {
                BorderRootEffect.Show();
                //BorderEffect.Collapse();
                //BorderTitle.Collapse();
                GridMain.HorizontalAlignment = HorizontalAlignment.Stretch;
                GridMain.VerticalAlignment = VerticalAlignment.Stretch;
                //PresenterMain.Margin = new Thickness();
                BorderRoot.CornerRadius = new CornerRadius(10);
                BorderRoot.Style = ResourceHelper.GetResource<Style>("BorderClip");
            }
            else
            {
                BorderRootEffect.Collapse();
                //BorderEffect.Show();
                //BorderTitle.Show();
                GridMain.HorizontalAlignment = HorizontalAlignment.Center;
                GridMain.VerticalAlignment = VerticalAlignment.Center;
                //PresenterMain.Margin = new Thickness(0, 0, 0, 10);
                BorderRoot.CornerRadius = new CornerRadius();
                BorderRoot.Style = null;
            }
        }

        private void SkinUpdated(SkinType skinType)
        {
            if (!_drawerCodeUsed)
            {
                return;
            }

            //_textEditor[ConstString.Xaml].SyntaxHighlighting = HighlightingProvider.GetDefinition(skinType, ConstString.Xml);
            //_textEditor[ConstString.Cs].SyntaxHighlighting = HighlightingProvider.GetDefinition(skinType, ConstString.Cs);
            //_textEditor[ConstString.Vm].SyntaxHighlighting = HighlightingProvider.GetDefinition(skinType, ConstString.Cs);
        }

        private void InitTextEditor()
        {
            //HighlightingProvider.Register(SkinType.Default, HighlightingProvider.Default);
            //HighlightingProvider.Register(SkinType.Dark, new HighlightingProviderDark());
            //HighlightingProvider.Register(SkinType.Violet, HighlightingProvider.Default);

            var textEditorCustomStyle = ResourceHelper.GetResource<Style>("TextEditorCustom");
            var skinType = GlobalData.Config?.Skin;

            //_textEditor = new Dictionary<string, TextEditor>
            //{
            //    [ConstString.Xaml] = new()
            //    {
            //        Style = textEditorCustomStyle,
            //        SyntaxHighlighting = HighlightingProvider.GetDefinition(skinType, ConstString.Xml)
            //    },
            //    [ConstString.Cs] = new()
            //    {
            //        Style = textEditorCustomStyle,
            //        SyntaxHighlighting = HighlightingProvider.GetDefinition(skinType, ConstString.Cs)
            //    },
            //    [ConstString.Vm] = new()
            //    {
            //        Style = textEditorCustomStyle,
            //        SyntaxHighlighting = HighlightingProvider.GetDefinition(skinType, ConstString.Cs)
            //    }
            //};

            //BorderCode.Child = new TabControl
            //{
            //    Style = ResourceHelper.GetResource<Style>("TabControl.PreviewCode"),
            //    Items =
            //{
            //    new TabItem
            //    {
            //        Header = ConstString.Xaml,
            //        //Content = _textEditor[ConstString.Xaml]
            //    },
            //    new TabItem
            //    {
            //        Header = ConstString.Cs,
            //        //Content = _textEditor[ConstString.Cs]
            //    },
            //    new TabItem
            //    {
            //        Header = ConstString.Vm,
            //        //Content = _textEditor[ConstString.Vm]
            //    }
            //}
            //};
        }

        private void UpdateTextEditor()
        {
            var typeKey = ViewModelLocator.Instance.Main.WorkspaceInfoCurrent?.Key;
            var demoKey = ViewModelLocator.Instance.Main.WorkspaceItemCurrent?.TargetCtlName;
            if (Equals(_currentDemoKey, demoKey))
            {
                return;
            }

            _currentDemoKey = demoKey;

            if (ViewModelLocator.Instance.Main.SubContent is FrameworkElement demoCtl)
            {
                var demoCtlTypeName = demoCtl.GetType().Name;
                var xamlPath = $"UserControl/{typeKey}/{demoCtlTypeName}.xaml";
                var dc = demoCtl.DataContext;
                var dcTypeName = dc.GetType().Name;
                var vmPath = !Equals(dcTypeName, demoCtlTypeName)
                    ? $"ViewModel/{dcTypeName}"
                    : xamlPath;

                //_textEditor[ConstString.Xaml].Text = DemoHelper.GetCode(xamlPath);
                //_textEditor[ConstString.Cs].Text = DemoHelper.GetCode($"{xamlPath}.cs");
                //_textEditor[ConstString.Vm].Text = DemoHelper.GetCode($"{vmPath}.cs");
            }
        }

        private void DrawerCode_OnOpened(object sender, RoutedEventArgs e)
        {
            if (!_drawerCodeUsed)
            {
                InitTextEditor();
                _drawerCodeUsed = true;
            }

            UpdateTextEditor();
        }

        private static class ConstString
        {
            public const string Xaml = "XAML";

            public const string Vm = "VM";

            public const string Cs = "C#";

            public const string Xml = "XML";
        }
    }
}
