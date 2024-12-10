using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using XStudio.Clouds;
using XStudio.Helpers;
using XStudio.Models;
using XStudio.Models.Requests;

namespace XStudio.Controllers.Clouds {

    [Route("api/xstudio/[controller]/v{version:apiVersion}")]
    [ApiVersion(1.0)]
    [ApiController]
    public class TencentCloudController : AbpController {
        private readonly ITencentCloudService _tencentCloudService;

        public TencentCloudController(ITencentCloudService tencentCloudService) {
            _tencentCloudService = tencentCloudService;
        }

        /// <summary>
        /// 获取腾讯云凭证
        /// </summary>
        /// <returns></returns>
        [HttpGet("getCredentials")]
        public async Task<ActionResult<TencentCloudDto?>> GetCredentialsAsync() {
            TencentCloudDto? tencentCloud = await _tencentCloudService.GetCredentialsAsync();
            if(tencentCloud != null) {
                Log.Information(tencentCloud.ToJson() ?? "获取腾讯云凭证失败");
                return CommonResult<TencentCloudDto?>.Success(tencentCloud);
            }
            return CommonResult<TencentCloudDto?>.Fail("获取腾讯云凭证失败");
        }

        /// <summary>
        /// 上传文件到腾讯云
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UploadFileDto>> UploadFileAsync(IFormFile file) {
            try {
                TencentCloudDto? qCloud = await _tencentCloudService.GetCredentialsAsync();
                if(qCloud == null) {
                    return CommonResult<UploadFileDto>.Fail("获取腾讯云凭证失败");
                }
                
                if(file == null) {
                    if(HttpContext.Request.Form.Files == null || !HttpContext.Request.Form.Files.Any()) {
                        return CommonResult<UploadFileDto>.Fail("没有获取到需要上传的文件");
                    }
                    file = HttpContext.Request.Form.Files[0];
                }
                UploadFileDto fileInfo = new UploadFileDto();
                fileInfo.Form = file;
                if(string.IsNullOrEmpty(fileInfo.FileName)) {
                    fileInfo.FileName = Path.GetFileName(file.FileName);
                }
                if(string.IsNullOrEmpty(fileInfo.FileType)) {
                    fileInfo.FileType = Path.GetExtension(file.FileName);
                }
                if(string.IsNullOrEmpty(fileInfo.FileName) || string.IsNullOrEmpty(fileInfo.FileType)) {
                    return CommonResult<UploadFileDto>.Fail("无法识别上传文件的名称或类型");
                }
                fileInfo.FileKey = $"{Guid.NewGuid().ToString("N")}{fileInfo.FileType}";

                Tuple<bool, string> result = await _tencentCloudService.UploadFileAsync(file, qCloud, fileInfo);
                if(result.Item1) {
                    return CommonResult<UploadFileDto>.Success(fileInfo);
                }
                return CommonResult<UploadFileDto>.Fail(result.Item2);
            }
            catch(Exception ex) {
                return CommonResult<UploadFileDto>.Fail(ex.Message);
            }
        }
    }
}
