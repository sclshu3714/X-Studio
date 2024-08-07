using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.App.Common
{
    /// <summary>
    /// 应用程序配置文件
    /// </summary> 
    public class AppSettings
    {
        private bool isDarkTheme = true;
        private string themeName = "Material";

        static AppSettings()
        {
            Instance = new AppSettings();
        }

        public static AppSettings Instance { get; }

        public bool IsDarkTheme
        {
            get => isDarkTheme;
            set
            {
                isDarkTheme = value;
            }
        }

        public string ThemeName
        {
            get => themeName;
            set { themeName = value; }
        }

        public static void OnInitialized()
        {
            //Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            ////Syncfusion LicenseKey
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cWmhAYVdpR2Nbe05xdF9CY1ZSRGYuP1ZhSXxXdk1jXH5edXZRRWlcWE0=");
        }
    }
}
