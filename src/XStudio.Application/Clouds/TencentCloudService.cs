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
        public async Task<TencentCloudDto?> GetCredentialsAsync() {
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
                    "name/cos:PutObject",
                    "name/cos:PostObject",
                    "name/cos:InitiateMultipartUpload",
                    "name/cos:ListMultipartUploads",
                    "name/cos:ListParts",
                    "name/cos:UploadPart",
                    "name/cos:CompleteMultipartUpload"
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
                values.Add("durationSeconds", 1800);//指定临时证书的有效期, 参考 https://cloud.tencent.com/document/product/1312/48195

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
                    TencentCloudDto credentialDto = new TencentCloudDto();
                    credentialDto.credentials = (credential["Credentials"] as JObject)?.ToObject<Credentials>() ?? new Credentials();
                    credentialDto.expiredTime = Convert.ToInt64(credential["ExpiredTime"]?.ToString() ?? "0");
                    credentialDto.expiration = credential["Expiration"]?.ToString() ?? "0";
                    credentialDto.requestId = credential["RequestId"]?.ToString() ?? "0";
                    credentialDto.startTime = Convert.ToInt64(credential["StartTime"]?.ToString() ?? "0");
                    return await Task.FromResult(credentialDto);
                }
                catch(Exception ex) {
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
        public async Task<Tuple<bool, string>> UploadFileAsync(IFormFile form, TencentCloudDto tencent, UploadFileDto file) {
            Tuple<bool, CosXmlServer?> tuple = await InitCosXmlAsync(tencent, file);
            if(tuple.Item1 && tuple.Item2 is CosXmlServer cosXml) {
                //_tencentCloudClient.InitCosXml(cosXml);
                return await UploadFileAsync(form, cosXml, file);
            }
            return Tuple.Create<bool, string>(true, "获取授权信息失败");
        }

        #region 私有方法 不公开
        /// <summary>
        /// 初始化COS服务实例
        /// </summary>
        private async Task<Tuple<bool, CosXmlServer?>> InitCosXmlAsync(TencentCloudDto dto, UploadFileDto fileDto) {
            if(string.IsNullOrEmpty(fileDto.Region) || string.IsNullOrEmpty(fileDto.Bucket) ||
               string.IsNullOrEmpty(dto.credentials?.tmpSecretId) || string.IsNullOrEmpty(dto.credentials?.tmpSecretKey) || string.IsNullOrEmpty(dto.credentials?.sessionToken)) {
                return Tuple.Create<bool, CosXmlServer?>(false, null);
            }
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(fileDto.Region) // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                .SetDebugLog(true) // 设置开启日志, 并在日志中查看请求相关信息
                .Build();
            //string tmpSecretId, string tmpSecretKey, long keyStartTimeSecond, long tmpExpiredTime, string sessionToken
            var qCloudCredentialProvider = new CustomQCloudCredentialProvider(dto.credentials.tmpSecretId,
                                                                              dto.credentials.tmpSecretKey,
                                                                              dto.credentials.sessionToken,
                                                                              dto.startTime,
                                                                              dto.expiredTime);
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
        private async Task<Tuple<bool, string>> UploadFileAsync(IFormFile form, CosXmlServer cosXml, UploadFileDto file) {
            try {
                if(form == null) {
                    return Tuple.Create<bool, string>(false, "上传文件不能为空");
                }
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = file.Bucket; // "examplebucket-1250000000";
                string key = file.FileKey; // "exampleobject"; //对象键
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
                    //关闭文件流
                    fileStream.Close();
                    //打印请求结果
                    Console.WriteLine(result.GetResultInfo());
                    return await Task.FromResult(Tuple.Create(true, "上传成功"));
                }
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Log.Error(clientEx, "上传文件到腾讯云对象存储失败");
                return Tuple.Create(false, clientEx.Message);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Log.Error(serverEx, "上传文件到腾讯云对象存储失败");
                return Tuple.Create(false, serverEx.GetInfo());
            }
        }
        #endregion

    }
}
