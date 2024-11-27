using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Settings;
using XStudio.Settings;

namespace XStudio.Themes {
    [RemoteService(false)]
    public class ThemeService : ApplicationService, IThemeService {
        private readonly ISettingDefinitionContext _settingProvider;

        public ThemeService(ISettingDefinitionContext settingProvider) {
            _settingProvider = settingProvider;
        }

        public async Task<string?> GetThemeAsync() {
            // 获取用户当前的主题设置
            SettingDefinition? theme = _settingProvider.GetOrNull(XStudioSettings.ThemeSetting);

            // 这里可以添加逻辑处理 theme，例如返回主题名称，或根据主题进行特定操作
            await Task.CompletedTask;
            return theme?.DefaultValue ?? string.Empty;
        }

        public async Task SetThemeAsync(string newTheme) {
            // 设置用户的新主题
            SettingDefinition? theme = _settingProvider.GetOrNull(XStudioSettings.ThemeSetting);
            if (theme != null) {
                theme.DefaultValue = newTheme;
            }
            await Task.CompletedTask;
        }
    }
}
