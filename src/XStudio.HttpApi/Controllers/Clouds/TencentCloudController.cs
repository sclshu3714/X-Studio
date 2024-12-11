using Asp.Versioning;
using COSXML.Transfer;
using COSXML;
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
using XStudio.Schools.Timetable;

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
        public async Task<ActionResult<CommonResult<TencentCloudCredentialsDto?>>> GetCredentialsAsync() {
            TencentCloudCredentialsDto? tencentCloud = await _tencentCloudService.GetCredentialsAsync();
            if(tencentCloud != null) {
                Log.Information(tencentCloud.ToJson() ?? "获取腾讯云凭证失败");
                return CommonResult<TencentCloudCredentialsDto?>.Success(tencentCloud);
            }
            return CommonResult<TencentCloudCredentialsDto?>.Fail("获取腾讯云凭证失败");
        }

        /// <summary>
        /// 上传文件到腾讯云
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CommonResult<UploadFileResultDto>>> UploadFileAsync(IFormFile file) {
            try {
                if(file == null) {
                    if(HttpContext.Request.Form.Files == null || !HttpContext.Request.Form.Files.Any()) {
                        return CommonResult<UploadFileResultDto>.Fail("没有获取到需要上传的文件");
                    }
                    file = HttpContext.Request.Form.Files[0];
                }
                if(file.Length > 10 * 1024 * 1024) {
                    return CommonResult<UploadFileResultDto>.Fail("单个文件大小不能超过10M");
                }
                TencentCloudCredentialsDto? qCloud = await _tencentCloudService.GetCredentialsAsync();
                if(qCloud == null) {
                    return CommonResult<UploadFileResultDto>.Fail("获取腾讯云凭证失败");
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
                    return CommonResult<UploadFileResultDto>.Fail("无法识别上传文件的名称或类型");
                }
                fileInfo.FileKey = $"{Guid.NewGuid().ToString("N")}{fileInfo.FileType}";
                UploadFileResultDto resultInfo = await _tencentCloudService.UploadFileAsync(file, qCloud, fileInfo);
                if(resultInfo.Success) {
                    resultInfo.FileUrl = $"https://{qCloud.Bucket}.cos.{qCloud.Region}.myqcloud.com/{fileInfo.FileKey}";
                    return CommonResult<UploadFileResultDto>.Success(resultInfo);
                }
                return CommonResult<UploadFileResultDto>.Fail(resultInfo.Message);
            }
            catch(Exception ex) {
                return CommonResult<UploadFileResultDto>.Fail(ex.Message);
            }
        }


        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <param name="files">上传文件列表 </param>
        [HttpPost("upload/batch")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<CommonResult<List<UploadFileResultDto>>>> BatchUploadAsync(List<IFormFile> files) {
            try {
                if(files == null) {
                    if(HttpContext?.Request?.Form?.Files == null || !HttpContext.Request.Form.Files.Any()) {
                        return CommonResult<List<UploadFileResultDto>>.Fail("没有获取到需要上传的文件");
                    }
                    files = HttpContext.Request.Form.Files.ToList();
                }
                if(files.Count > 3 || files.Any(f => f.Length > 10 * 1024 * 1024)) {
                    return CommonResult<List<UploadFileResultDto>>.Fail("批量上传文件数量不能超过3个，单个文件大小不能超过10M");
                }
                TencentCloudCredentialsDto? qCloud = await _tencentCloudService.GetCredentialsAsync();
                if(qCloud == null) {
                    return CommonResult<List<UploadFileResultDto>>.Fail("获取腾讯云凭证失败");
                }
                var uploadTasks = files.Select<IFormFile, Task<UploadFileResultDto>>(async file => {
                    UploadFileDto fileInfo = new UploadFileDto();
                    fileInfo.Form = file;
                    if(string.IsNullOrEmpty(fileInfo.FileName)) {
                        fileInfo.FileName = Path.GetFileName(file.FileName);
                    }
                    if(string.IsNullOrEmpty(fileInfo.FileType)) {
                        fileInfo.FileType = Path.GetExtension(file.FileName);
                    }
                    if(string.IsNullOrEmpty(fileInfo.FileName) || string.IsNullOrEmpty(fileInfo.FileType)) {
                        return new UploadFileResultDto() {
                            Success = false,
                            Message = "无法识别上传文件的名称或类型",
                        };
                    }
                    fileInfo.FileKey = $"{Guid.NewGuid().ToString("N")}{fileInfo.FileType}";
                    UploadFileResultDto resultInfo = await _tencentCloudService.UploadFileAsync(file, qCloud, fileInfo);
                    if(resultInfo.Success) {
                        resultInfo.FileUrl = $"https://{qCloud.Bucket}.cos.{qCloud.Region}.myqcloud.com/{fileInfo.FileKey}";
                        return resultInfo;
                    }
                    return resultInfo;
                });
                var results = await Task.WhenAll(uploadTasks);
                return CommonResult<List<UploadFileResultDto>>.Success(results.ToList());
            }
            catch(Exception ex) {
                Log.Error(ex, "批量上传文件失败");
                return CommonResult<List<UploadFileResultDto>>.Fail(ex.Message);
            }
        }
    }
}
