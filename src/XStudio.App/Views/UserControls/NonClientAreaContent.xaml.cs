using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Data;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace XStudio.App.Views.UserControls
{
    /// <summary>
    /// NonClientAreaContent.xaml 的交互逻辑
    /// </summary>
    public partial class NonClientAreaContent : UserControl
    {
        public NonClientAreaContent()
        {
            InitializeComponent();
        }

        private void ButtonLangs_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button { Tag: string langName })
            {
                PopupConfig.IsOpen = false;
                if (langName.Equals(GlobalData.Config.Lang))
                {
                    return;
                }

                ConfigHelper.Instance.SetLang(langName);
                //XStudio.App.Properties.Langs.LangProvider.Culture = new CultureInfo(langName);
                //Messenger.Default.Send<object>(null, MessageToken.LangUpdated);

                GlobalData.Config.Lang = langName;
                GlobalData.Save();
            }
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e) => PopupConfig.IsOpen = true;

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button { Tag: SkinType skinType })
            {
                PopupConfig.IsOpen = false;
                if (skinType.Equals(GlobalData.Config.Skin))
                {
                    return;
                }

                GlobalData.Config.Skin = skinType;
                GlobalData.Save();
                //((App)Application.Current).UpdateSkin(skinType);
                //Messenger.Default.Send(skinType, MessageToken.SkinUpdated);
            }
        }

        private void MenuAbout_OnClick(object sender, RoutedEventArgs e)
        {
            //new AboutWindow
            //{
            //    Owner = Application.Current.MainWindow
            //}.ShowDialog();
        }
    }
}
