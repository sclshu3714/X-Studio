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
        private string rootUrl = "https://localhost:44345";
        private readonly static object lockObject = new object();
        private static AppSettings? _Instance;
        public static AppSettings Instance 
        {
            get {
                if (_Instance == null) { 
                    lock (lockObject) {
                        if (_Instance == null) {
                            _Instance = new AppSettings();
                        }
                    }
                }
                return _Instance;
            }
        }

        public string RootUrl {
            get => rootUrl;
            set { rootUrl = value; }
        }

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

        public void OnInitialized()
        {
            //Syncfusion.SfSkinManager.SfSkinManager.ApplyStylesOnApplication = true;
            ////Syncfusion LicenseKey
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cWmhAYVdpR2Nbe05xdF9CY1ZSRGYuP1ZhSXxXdk1jXH5edXZRRWlcWE0=");
        }
    }
}
