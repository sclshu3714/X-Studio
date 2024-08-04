using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Volo.Abp.SettingManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using XStudio.Common;

namespace XStudio.ExtensionGrant
{
    [IgnoreAntiforgeryToken]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class WebTokenExtensionGrant : AbstractTokenExtensionGrant
    {
        public const string ExtensionGrantName = "XStudio_Web";
        public override string Name => ExtensionGrantName;

        protected override async Task<IResults> GetResultsAsync()
        {
            string? code = GrantContext?.Request?.GetParameter("code").ToString();
            if (string.IsNullOrEmpty(code))
            {
                return new IResults() { ErrorCode = 400, ErrorMessage = "code参数为空！" };
            }

            try
            {
                //WebExtensionService extService = new WebExtensionService(SettingManager);
                IResults _result;
                //二维码免登授权（企业内部）
                //var prResult = await extService.GetContactUsersAsync(code);
                //_result = prResult;
                //ProviderKey = prResult.Errcode == 0 ? prResult.Result.UnionId : "";
                return new IResults() { };
            }
            catch (Exception ex)
            {
                return new IResults { ErrorCode = 500, ErrorMessage = "接口请求失败." + ex.Message };
            }
        }
    }
}
