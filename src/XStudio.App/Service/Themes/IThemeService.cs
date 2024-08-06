using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XStudio.App.Models.Themes;

namespace XStudio.App.Service.Themes
{
    public interface IThemeService
    {
        ObservableCollection<ThemeItem> ThemeItems { get; set; }

        string GetCurrentName();

        void SetTheme(string themeName);

        void SetThemeMode();

        void SetCurrentTheme(DependencyObject dependency);
    }
}
