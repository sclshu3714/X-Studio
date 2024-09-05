using XStudio.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace XStudio.Permissions;

public class XStudioPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(XStudioPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(XStudioPermissions.MyPermission1, L("Permission:MyPermission1"));
        var projectsPermission = myGroup.AddPermission(
            XStudioPermissions.Projects.Default, L("Permission:Projects"));
        projectsPermission.AddChild(
            XStudioPermissions.Projects.Create, L("Permission:Projects.Create"));
        projectsPermission.AddChild(
            XStudioPermissions.Projects.Edit, L("Permission:Projects.Edit"));
        projectsPermission.AddChild(
            XStudioPermissions.Projects.Delete, L("Permission:Projects.Delete"));
        //school
        var schoolsPermission = myGroup.AddPermission(
            XStudioPermissions.Schools.Default, L("Permission:Schools"));
        schoolsPermission.AddChild(
            XStudioPermissions.Schools.Create, L("Permission:Schools.Create"));
        schoolsPermission.AddChild(
            XStudioPermissions.Schools.Edit, L("Permission:Schools.Edit"));
        schoolsPermission.AddChild(
            XStudioPermissions.Schools.Delete, L("Permission:Schools.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<XStudioResource>(name);
    }
}
