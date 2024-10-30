using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XStudio.App.Resources.Themes {
    public class XStudioTheme : Theme {
        public override ResourceDictionary GetSkin(SkinType skinType) =>
        ResourceHelper.GetSkin(typeof(App).Assembly, "Resources/Themes", skinType);

        public override ResourceDictionary GetTheme() => new() {
            Source = new Uri("pack://application:,,,/HandyControlDemo;component/Resources/Themes/Theme.xaml")
        };
    }
}
