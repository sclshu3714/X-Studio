using COSSTS;
using COSXML.Auth;
using COSXML;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentCloud.Ckafka.V20190819.Models;
using Volo.Abp.Application.Services;
using XStudio.Common;
using XStudio.Common.Helper;
using XStudio.Common.Clouds;
using COSXML.Common;
using COSXML.CosException;
using COSXML.Log;
using COSXML.Model;
using COSXML.Transfer;
using COSXML.Model.Object;
using System.IO;
using System.Net.Http;
using Serilog;
using Volo.Abp;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using static COSXML.Model.Tag.DocumentCensorResult;
using COSXML.Model.Tag;

namespace XStudio.Clouds {
    [RemoteService(false)]
    public class TencentCloudService : ApplicationService, ITencentCloudService {
        private readonly IConfiguration _configuration;
        public TencentCloudService(IConfiguration configuration) {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取腾讯云临时密钥
        /// </summary>
        /// <returns></returns>
        public async Task<TencentCloudCredentialsDto?> GetCredentialsAsync() {
            TencentCloudOption tencentCloudOption = new TencentCloudOption();
            _configuration.GetSection("CloudClient:TencentCloud").Bind(tencentCloudOption);
            if(tencentCloudOption.IsEnable) {
                string bucket = tencentCloudOption.Bucket; // 您的 bucket
                string region = tencentCloudOption.Region; // bucket 所在区域

                // 改成允许的路径前缀，根据自己网站的用户判断允许上传的路径，例子:a.jpg 或者 a/* 或者 * (通配符*存在重大安全风险, 谨慎评估使用)
                string allowPrefix = "*";

                /*
                 * 密钥的权限列表。必须在这里指定本次临时密钥所需要的权限。权限列表请参见 https://cloud.tencent.com/document/product/436/31923
                 * 规则为 {project}:{interfaceName}
                 * project : 产品缩写  cos相关授权为值为cos,数据万象(数据处理)相关授权值为ci
                 * 授权所有接口用*表示，例如 cos:*,ci:*
                 */
                string[] allowActions = new string[]
                {
                    // 简单上传操作 
                    "name/cos:PutObject",
                    // 表单上传对象 
                    "name/cos:PostObject",
                    // 分块上传：初始化分块操作 
                    "name/cos:InitiateMultipartUpload",
                    // 分块上传：List 进行中的分块上传
                    "name/cos:ListMultipartUploads",
                    // 分块上传：List 已上传分块操作 
                    "name/cos:ListParts",
                    // 分块上传：上传分块操作 
                    "name/cos:UploadPart",
                    // 分块上传：完成所有分块上传操作 
                    "name/cos:CompleteMultipartUpload",
                    // 取消分块上传操作 
                    "name/cos:AbortMultipartUpload",
                    // 查询对象元数据
                    "name/cos:HeadObject",
                    // 下载操作 
                    "name/cos:GetObject",
                    // 删除单个对象
                    "name/cos:DeleteObject",
                    // 删除多个对象
                    "name/cos:DeleteObject",
                    // 授予所有资源只读权限
                    "name/cos:OptionsObject",
                    // 查询存储桶列表
                    "name/cos:GetService",
                    // 创建存储桶
                    "name/cos:PutBucket",
                    // 检索存储桶及其权限
                    "name/cos:HeadBucket",
                    // 查询对象列表
                    "name/cos:GetBucket",
                    // 删除存储桶
                    "name/cos:DeleteBucket",
                    // 设置存储桶 ACL
                    "name/cos:PutBucketACL",
                    // 查询存储桶 ACL
                    "name/cos:GetBucketACL",
                    // 设置跨域配置
                    "name/cos:PutBucketCORS",
                    // 查询跨域配置
                    "name/cos:GetBucketCORS",
                    // 删除跨域配置
                    "name/cos:DeleteBucketCORS",
                    // 设置生命周期
                    "name/cos:PutBucketLifecycle",
                    // 查询生命周期
                    "name/cos:GetBucketLifecycle",
                    // 删除生命周期
                    "name/cos:DeleteBucketLifecycle",
                    // 所有操作
                    "name/cos:*"
                };

                //设置参数
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("bucket", bucket);
                values.Add("region", region);
                values.Add("allowPrefix", allowPrefix);
                // 也可以通过 allowPrefixes 指定路径前缀的集合
                // values.Add("allowPrefixes", new string[] {
                //     "path/to/dir1/*",
                //     "path/to/dir2/*",
                // });
                values.Add("allowActions", allowActions);
                values.Add("durationSeconds", tencentCloudOption.DurationSeconds);//指定临时证书的有效期, 参考 https://cloud.tencent.com/document/product/1312/48195

                // Demo 这里是从环境变量读取，如果是直接硬编码在代码中可以直接（ string secretId = "secretId-DSFser";)：
                string secretId = tencentCloudOption.SecretId; // 云 API 密钥 Id
                string secretKey = tencentCloudOption.SecretKey; // 云 API 密钥 Key
                if(tencentCloudOption.IsEncrypt) {
                    EncrypterHelper.EncryptionKey = "AIC_TENCENT_CLOUD_COS_ENCRYPTION_KEY";
                    secretId = EncrypterHelper.Decrypt(secretId);
                    secretKey = EncrypterHelper.Decrypt(secretKey);
                }
                values.Add("secretId", secretId);
                values.Add("secretKey", secretKey);

                // 设置域名
                // values.Add("Domain", "sts.tencentcloudapi.com");

                /*  STSClient.genCredential打印的返回值示例
                 *  Credentials = {
                 *     "Token": "4oztDXOAAI3c6qUE5TkNudfkmkjgnwlirngwjngmcwkfzSP...",
                 *     "TmpSecretId": "xxxxxxxxxxxx",
                 *     "TmpSecretKey": "PZ/WWfPZFYqahPSs8URUVMc8IyJH+T24zdn8V1cZaMs="
                 *   }
                 *   ExpiredTime = 1597916602
                 *   Expiration = 2020/8/20 上午9:43:22
                 *   RequestId = 2b731be1-ebe8-4638-8a72-906bc564a55a
                 *   StartTime = 1597914802
                 */
                try {
                    Dictionary<string, object> credential = STSClient.genCredential(values); //返回值说明见README.md
                    //foreach(KeyValuePair<string, object> kvp in credential) {
                    //    Console.WriteLine("{0} = {1}", kvp.Key, kvp.Value);
                    //}
                    TencentCloudCredentialsDto credentialDto = new TencentCloudCredentialsDto();
                    credentialDto.Credentials = (credential["Credentials"] as JObject)?.ToObject<Credentials>() ?? new Credentials();
                    credentialDto.ExpiredTime = Convert.ToInt64(credential["ExpiredTime"]?.ToString() ?? "0");
                    credentialDto.Expiration = credential["Expiration"]?.ToString() ?? "0";
                    credentialDto.RequestId = credential["RequestId"]?.ToString() ?? "0";
                    credentialDto.StartTime = Convert.ToInt64(credential["StartTime"]?.ToString() ?? "0");
                    return await Task.FromResult(credentialDto);
                }
                catch(Exception ex) {
                    Log.Error(ex, "获取腾讯云临时密钥失败");
                    return null;
                }
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// 上传文件到腾讯云对象存储
        /// </summary>
        /// <param name="tencent"></param>
        /// <param name="file"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UploadFileResultDto> UploadFileAsync(IFormFile form, TencentCloudCredentialsDto tencent, UploadFileDto file) {
            Tuple<bool, CosXmlServer?> tuple = await InitCosXmlAsync(tencent, file);
            if(tuple.Item1 && tuple.Item2 is CosXmlServer cosXml) {
                //_tencentCloudClient.InitCosXml(cosXml);
                return await UploadFileAsync(form, cosXml, file);
            }
            return new UploadFileResultDto() {
                Success = false,
                Message = "初始化COS服务实例失败",
                FileKey = file.FileKey,
                FileType = file.FileType,
                FileName = file.FileName
            };
        }

        /// <summary>
        /// 当对象 ACL 属性设置为“公有读”时，可以通过以下 SDK 接口生成的 URL 直接访问对象（仅支持生成 COS 默认源站域名的 URL）。
        /// </summary>
        /// <param name="cosXml"></param>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        public string? GetObjectUrl(CosXmlServer cosXml, string bucket, string key) {
            try {
                string url = cosXml.GetObjectUrl(bucket, key);
                Console.WriteLine("Object Url is: " + url);
                return url;
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                return null;
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                return null;
            }
        }

        public string? GetPreSignDownloadUrl(CosXmlServer cosXml,string region, string bucket, string key,long signDurationSecond = 12 * 60 * 60) {
            try {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = bucket.Substring(bucket.IndexOf("-") + 1);//"1250000000"; //腾讯云账号 APPID
                preSignatureStruct.region = region;//"COS_REGION"; //存储桶地域
                preSignatureStruct.bucket = bucket;// "examplebucket-1250000000"; //存储桶
                preSignatureStruct.key = key; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = signDurationSecond; //请求签名时间为600s
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数
                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine(requestSignURL);
                return requestSignURL;
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                return null;
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                return null;
            }
        }
        #region 私有方法 不公开
        /// <summary>
        /// 初始化COS服务实例
        /// </summary>
        private async Task<Tuple<bool, CosXmlServer?>> InitCosXmlAsync(TencentCloudCredentialsDto dto, UploadFileDto fileDto) {
            string? sessionToken = string.IsNullOrWhiteSpace(dto.Credentials?.SessionToken) ? dto.Credentials?.Token : string.Empty;
            if(string.IsNullOrEmpty(fileDto.Region) || string.IsNullOrEmpty(fileDto.Bucket) ||
               string.IsNullOrEmpty(dto.Credentials?.TmpSecretId) || string.IsNullOrEmpty(dto.Credentials?.TmpSecretKey) || 
               string.IsNullOrEmpty(sessionToken)) {
                return Tuple.Create<bool, CosXmlServer?>(false, null);
            }
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(fileDto.Region) // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                .SetDebugLog(true) // 设置开启日志, 并在日志中查看请求相关信息
                .Build();
            //string tmpSecretId, string tmpSecretKey, long keyStartTimeSecond, long tmpExpiredTime, string sessionToken
            var qCloudCredentialProvider = new CustomQCloudCredentialProvider(dto.Credentials.TmpSecretId,
                                                                              dto.Credentials.TmpSecretKey,
                                                                              sessionToken,
                                                                              dto.StartTime,
                                                                              dto.ExpiredTime);
            CosXmlServer cosXml = new CosXmlServer(config, qCloudCredentialProvider);
            return await Task.FromResult(Tuple.Create<bool, CosXmlServer?>(true, cosXml));
        }

        /// <summary>
        /// 上传文件到腾讯云对象存储
        /// </summary>
        /// <param name="cosXml"></param>
        /// <param name="file"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        private async Task<UploadFileResultDto> UploadFileAsync(IFormFile form, CosXmlServer cosXml, UploadFileDto file) {
            UploadFileResultDto resultInfo = new UploadFileResultDto();
            resultInfo.FileKey = file.FileKey;
            resultInfo.FileType = file.FileType;
            resultInfo.FileName = file.FileName;
            try {
                if(form == null) {
                    resultInfo.Success = false;
                    resultInfo.Message = "上传文件不能为空";
                    return resultInfo;
                }
                resultInfo.FileSize = form.Length;
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = file.Bucket; // "examplebucket-1250000000";
                string key = file.FileKey;   // "exampleobject"; //对象键
                // 打开只读的文件流对象
                using(Stream fileStream = form.OpenReadStream()) {
                    // 组装上传请求，其中 offset sendLength 为可选参数
                    long offset = 0L;
                    long sendLength = fileStream.Length;

                    PutObjectRequest request = new PutObjectRequest(bucket, key, fileStream, offset, sendLength);
                    //设置进度回调
                    request.SetCosProgressCallback(delegate (long completed, long total) {
                        Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                    });
                    //执行请求
                    PutObjectResult result = cosXml.PutObject(request);
                    resultInfo.ObjectUrl = cosXml.GetObjectUrl(bucket, key);
                    resultInfo.PreSignDownloadUrl = GetPreSignDownloadUrl(cosXml, file.Region, bucket, key);
                    //打印请求结果
                    Console.WriteLine(result.GetResultInfo());
                    resultInfo.Message = result.GetResultInfo();
                    resultInfo.Success = true;
                    return await Task.FromResult(resultInfo);
                }
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Log.Error(clientEx, "上传文件到腾讯云对象存储失败");
                resultInfo.Message = clientEx.Message;
                resultInfo.Success = false;
                return resultInfo;
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Log.Error(serverEx, "上传文件到腾讯云对象存储失败");
                resultInfo.Message = serverEx.Message;
                resultInfo.Success = false;
                return resultInfo;
            }
        }

        
        #endregion

    }
}
