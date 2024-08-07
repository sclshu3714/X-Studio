using DryIoc.ImTools;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XStudio.App.Common;
using XStudio.App.Extensions;
using XStudio.App.Models.Themes;

namespace XStudio.App.Service.Themes
{
    public class ThemeService : BindableBase, IThemeService
    {
        private bool isDarkTheme = false;
        private ObservableCollection<ThemeItem> themeItems = new ObservableCollection<ThemeItem>();
        public ThemeService()
        {
            ThemeItems = new ObservableCollection<ThemeItem>()
            {
                new ThemeItem()
                {  
                    DisplayName="Fluent", 
                    LightName="FluentLight",
                    DarkName="FluentDark"
                },
                new ThemeItem()
                {  
                    DisplayName="Material",
                    LightName="MaterialLight",
                    DarkName="MaterialDark"
                },
                new ThemeItem()
                {  
                    DisplayName="MaterialBlue",
                    LightName="MaterialLightBlue",
                    DarkName="MaterialDarkBlue"
                },
                new ThemeItem()
                {  
                    DisplayName="Office2019",
                    LightName="Office2019White",
                    DarkName="Office2019Black"
                }
            };

            IsDarkTheme = AppSettings.Instance.IsDarkTheme;
        }

        

        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set { isDarkTheme = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<ThemeItem> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; RaisePropertyChanged(); }
        }

        public void SetTheme(string displayName)
        {
            AppSettings.Instance.ThemeName = displayName;
            SetThemeInternal(GetCurrentName());
        }

        public void SetThemeMode()
        {
            IsDarkTheme = !IsDarkTheme;
            AppSettings.Instance.IsDarkTheme = IsDarkTheme;
            SetThemeInternal(GetCurrentName());
        }

        private void SetThemeInternal(string themeName)
        {
            App.Current.MainWindow.SetTheme(themeName);
        }

        public void SetCurrentTheme(DependencyObject dependency)
        {
            dependency.SetTheme(GetCurrentName());
        }

        public string GetCurrentName()
        {
            var item = ThemeItems.FindFirst(t => t.DisplayName.Equals(AppSettings.Instance.ThemeName));
            return AppSettings.Instance.IsDarkTheme ? item.DarkName : item.LightName;
        }
    }
}
