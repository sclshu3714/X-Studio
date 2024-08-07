using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace XStudio.App.Extensions
{
    public static class SkinManagerExtensions
    {
        public static void SetTheme(
            this DependencyObject dependencyObject,
            string themeName)
        {
            //IThemeSetting theme = GetThemeSetting(themeName);
            //var themeTypeName = theme.GetType().Name.Replace("ThemeSettings", "");
            //SfSkinManager.RegisterThemeSettings(themeTypeName, theme);
            //SfSkinManager.SetTheme(dependencyObject, new Theme(themeTypeName));
        }

        //private static IThemeSetting GetThemeSetting(string themeName)
        //{
        //    FontFamily fontFamily = new FontFamily("Microsoft YaHei");
        //    int bodyfontSize = 14;
        //    IThemeSetting theme = null;
        //    switch (themeName)
        //    {
        //        case "FluentLight":
        //            //theme = new FluentLightThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "MaterialLight":
        //            //theme = new MaterialLightThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "MaterialLightBlue":
        //            //theme = new MaterialLightBlueThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "Office2019White":
        //            //theme = new Office2019WhiteThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "FluentDark":
        //            //theme = new FluentDarkThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "MaterialDark":
        //            //theme = new MaterialDarkThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "MaterialDarkBlue":
        //            //theme = new MaterialDarkBlueThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;

        //        case "Office2019Black":
        //        default:
        //            //theme = new Office2019BlackThemeSettings() { FontFamily = fontFamily, BodyFontSize = bodyfontSize, };
        //            break;
        //    }
        //    return theme;
        //}
    }
}
