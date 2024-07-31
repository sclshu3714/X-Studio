using Volo.Abp.Settings;

namespace XStudio.Settings;

public class XStudioSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(XStudioSettings.MySetting1));
    }
}
