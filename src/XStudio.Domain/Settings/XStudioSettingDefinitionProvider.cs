using Volo.Abp.Settings;

namespace XStudio.Settings;

public class XStudioSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(XStudioSettings.MySetting1));

        // 示例：定义一个主题设置项
        context.Add(new SettingDefinition(
            XStudioSettings.ThemeSetting, // 使用定义的设置项
            "DefaultTheme", // 默认值
            isVisibleToClients: true // 是否对客户端可见
        ));
    }
}
