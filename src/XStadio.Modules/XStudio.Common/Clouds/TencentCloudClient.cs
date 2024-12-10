using COSXML.Auth;
using COSXML;
using Microsoft.AspNetCore.Server.HttpSys;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using COSXML.Transfer;
using COSXML.Model.Object;
using static COSXML.Model.Tag.ListAllMyBuckets;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
using COSXML.Model;
using COSXML.CosException;
using XStudio.Common.Helper;
using Volo.Abp.DependencyInjection;
using Microsoft.AspNetCore.DataProtection;

namespace XStudio.Common.Clouds {
    public class TencentCloudClient : ITransientDependency {
        private static object locker = new object();
        private static TencentCloudClient? instance;
        public static TencentCloudClient Instance {
            get {
                if(instance == null) {
                    lock(locker) {
                        if(instance == null) {
                            instance = new TencentCloudClient();
                            //instance.InitCosXml();
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// SecretId
        /// </summary>
        public string SecretId { get; set; } = string.Empty;

        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 临时密钥
        /// </summary>
        public string SessionToken { get; set; } = string.Empty;

        /// <summary>
        /// COS服务地域
        /// </summary>
        public string Region { get; set; } = "ap-chengdu";

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string Bucket { get; set; } = "jf-aic-prod";

        /// <summary>
        /// 初始化COS服务实例
        /// </summary>
        public CosXml? cosXml { get; set; } //将服务用户设置成数据成员

        /// <summary>
        /// 进度回调
        /// </summary>
        /// <param name="completed"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public Action<long, long>? OnProgressCallbackAction { get; set; }

        public TencentCloudClient() {
        }

        /// <summary>
        /// 返回指示文件是否已被其它程序使用的布尔值
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名，例如：“C:\MyFile.txt”。</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static bool FileIsUsed(string fileFullName) {
            bool result = false;

            //判断文件是否存在，如果不存在，直接返回 false
            if(!File.Exists(fileFullName)) {
                result = false;

            }//end: 如果文件不存在的处理逻辑
            else {//如果文件存在，则继续判断文件是否已被其它程序使用
                //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream? fileStream = null;
                try {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);

                    result = false;
                }
                catch(System.IO.IOException) {
                    result = true;
                }
                catch(System.Exception) {
                    result = true;
                }
                finally {
                    fileStream?.Close();
                }

            }//end: 如果文件存在的处理逻辑

            //返回指示文件是否已被其它程序使用的值
            return result;

        }


        /// <summary>
        /// 初始化COS服务实例
        /// </summary>
        public void InitCosXml(string region, string bucket, string tmpSecretId, string tmpSecretKey, string sessionToken,long startTime, long tmpExpiredTime, bool isEncrypt = false) {
            Region = region;
            SecretId = tmpSecretId;
            SecretKey = tmpSecretKey;
            Bucket = bucket;
            if(isEncrypt) {
                tmpSecretId = EncrypterHelper.Decrypt(tmpSecretId);
                tmpSecretKey = EncrypterHelper.Decrypt(tmpSecretKey);
                sessionToken = EncrypterHelper.Decrypt(sessionToken);
            }
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region) // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                .Build();
            var qCloudCredentialProvider = new DefaultSessionQCloudCredentialProvider(tmpSecretId, tmpSecretKey, startTime, tmpExpiredTime, sessionToken);
            this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }
        public void InitCosXml(CosXmlServer cosXml) { 
            this.cosXml = cosXml;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="bucket"></param>
        public void CreateDir(string bucket, string dir) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000";
                string cosPath = dir;// "dir/"; // 对象键
                PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, new byte[0]);
                PutObjectResult? result = cosXml?.PutObject(putObjectRequest);
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        //当对象 ACL 属性设置为“公有读”时，可以通过以下 SDK 接口生成的 URL 直接访问对象（仅支持生成 COS 默认源站域名的 URL）。
        public string? GetObjectUrl(string bucket, string key) {
            try {
                //// 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                //string bucket = "examplebucket-1250000000";
                //string key = "exampleobject"; //对象键
                // 生成链接（默认域名访问）
                string? url = cosXml?.GetObjectUrl(bucket, key);
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

        public void GetPreSignDownloadUrl(string region, string bucket, string key) {
            if(cosXml == null)
                return;
            try {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = bucket.Substring(bucket.LastIndexOf('-'));//"1250000000"; //腾讯云账号 APPID
                preSignatureStruct.region = region;//"COS_REGION"; //存储桶地域
                preSignatureStruct.bucket = bucket;// "examplebucket-1250000000"; //存储桶
                preSignatureStruct.key = key; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数
                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine(requestSignURL);

                //下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                string localDir = System.IO.Path.GetTempPath(); //本地文件夹
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                //设置下载请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 生成预签名上传链接
        /// </summary>
        /// <param name="region"></param>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="srcPath"></param>
        public void GetPreSignUploadUrl(string region, string bucket, string key, string srcPath) {
            if(cosXml == null)
                return;
            try {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                // APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.appid = bucket.Substring(bucket.LastIndexOf('-'));//"1250000000";
                                                                                     // 存储桶所在地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                preSignatureStruct.region = region;//"COS_REGION";
                                                   // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.bucket = bucket;//"examplebucket-1250000000";
                preSignatureStruct.key = key; //对象键
                preSignatureStruct.httpMethod = "PUT"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为 600s
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数
                                                           //上传预签名 URL (使用永久密钥方式计算的签名 URL)
                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine(requestSignURL);

                //string srcPath = @"local-file-path"; //本地文件绝对路径
                PutObjectRequest request = new PutObjectRequest(null, null, srcPath);
                //设置上传请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult result = cosXml.PutObject(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 生成预签名 URL，并在签名中携带 Host
        /// </summary>
        /// <param name="region"></param>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="localDir"></param>
        /// <param name="localFileName"></param>
        public void GetPreSignUrlWithHost(string region, string bucket, string key, string localDir, string localFileName) {
            if(cosXml == null)
                return;
            try {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                // APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.appid = bucket.Substring(bucket.LastIndexOf('-'));
                // 存储桶所在地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                preSignatureStruct.region = region;
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.bucket = bucket;
                preSignatureStruct.key = key; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.signHost = true; // 请求中签入Host，建议开启，能够有效防止越权请求，需要注意，开启后实际请求也需要携带Host请求头
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数

                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine("requestUrl is:" + requestSignURL);

                ////下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                //string localDir = System.IO.Path.GetTempPath(); //本地文件夹
                //string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                //设置下载请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 生成预签名URL，并在签名中携带请求参数
        /// </summary>
        public void GetPreSignUrlWithReqParam(string region, string bucket, string key, string localDir, string localFileName) {
            if(cosXml == null)
                return;
            try {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                // APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.appid = bucket.Substring(bucket.LastIndexOf('-'));
                // 存储桶所在地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                preSignatureStruct.region = region;
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.bucket = bucket;
                preSignatureStruct.key = key; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.signHost = true; // 请求中签入Host，建议开启，能够有效防止越权请求，需要注意，开启后实际请求也需要携带Host请求头
                preSignatureStruct.headers = null; // 签名中需要校验的 header
                string ci_params = "imageMogr2/thumbnail/!50p";
                preSignatureStruct.queryParameters = new Dictionary<string, string>(); // 签名中需要校验的 URL 中请求参数，以请求万象图片处理为例
                preSignatureStruct.queryParameters.Add(ci_params, null);

                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine("requestUrl is:" + requestSignURL);

                ////下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                //string localDir = System.IO.Path.GetTempPath(); //本地文件夹
                //string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                //设置下载请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> UploadFileAsync(string bucket, string fileKey, string localFile) {
            if(cosXml == null)
                return Tuple.Create(false, "请先初始化cosXml");
            int index = 0;
            bool NoUpload = false;
            NoUpload = FileIsUsed(localFile);
            while(NoUpload && index <= 3) {
                await Task.Delay(1000);
                NoUpload = FileIsUsed(localFile);
                index++;
            }
            if(NoUpload) {
                return Tuple.Create(false, "上传失败，文件被占用");
            }
            try {
                TransferConfig transferConfig = new TransferConfig();
                // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
                transferConfig.DivisionForUpload = 10 * 1024 * 1024;
                // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
                transferConfig.SliceSizeForUpload = 2 * 1024 * 1024;
                // 初始化 TransferManager
                TransferManager transferManager = new TransferManager(cosXml, transferConfig);
                // 存储桶名称，此处填入格式必须为 BucketName-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // string bucket = "examplebucket-1250000000";
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";
                string cosPath = fileKey;//"exampleobject"; //对象在存储桶中的位置标识符，即称对象键
                string srcPath = localFile; // "temp-source-file";//本地文件绝对路径  
                // 上传对象 uploadTask.Pause(); //暂停上传 uploadTask.Resume(); //恢复上传 uploadTask.Cancel(); //取消上传
                COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
                uploadTask.SetSrcPath(srcPath);
                uploadTask.progressCallback = delegate (long completed, long total) {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                    OnProgressCallbackAction?.Invoke(completed, total);
                };
                uploadTask.successCallback = delegate (CosResult result) {
                    Console.WriteLine("success");
                };
                uploadTask.failCallback = delegate (CosClientException clientException, CosServerException serverException) {
                    Console.WriteLine("failed");
                };
                //开始上传
                COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);
                Console.WriteLine(result.GetResultInfo());
                return Tuple.Create(true, $"上传成功");
            }
            catch(Exception ex) {
                Log.Error($"上传失败: {ex.Message}");
                return Tuple.Create(false, $"上传失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <param name="Bucket">桶名</param>
        /// <param name="localFiles">上传文件列表 </param>
        public void BatchUpload(string bucket, Dictionary<string, string> localFiles) {
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            // var AppId = EncrypterHelper.Decrypt(APPID);
            // bucket = $"{bucket}-{AppId}";//"examplebucket-1250000000";
            foreach(var fileKey in localFiles.Keys) {
                // 上传对象
                string cosPath = fileKey;// "exampleobject" + i; //对象在存储桶中的位置标识符，即称对象键
                string srcPath = localFiles[fileKey]; // @"temp-source-file";//本地文件绝对路径
                COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
                uploadTask.SetSrcPath(srcPath);
                transferManager.UploadAsync(uploadTask).Wait();
            }
        }

        /// <summary>
        /// 本地上传
        /// </summary>
        public void PutObject(string bucket, string fileKey, string localFile) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";// examplebucket-1250000000";
                string key = fileKey;// "exampleobject"; //对象键
                string srcPath = localFile; // @"temp-source-file";//本地文件绝对路径

                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total) {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                PutObjectResult? result = cosXml?.PutObject(request);
                //打印返回结果
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 表单上传
        /// </summary>
        public void PostObject(string bucket, string fileKey, string localFile) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000";
                string key = fileKey;// "exampleobject"; //对象键
                string srcPath = localFile; // @"temp-source-file";//本地文件绝对路径
                PostObjectRequest request = new PostObjectRequest(bucket, key, srcPath);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total) {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PostObjectResult? result = cosXml?.PostObject(request);
                //请求成功
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 上传二进制数据
        /// </summary>
        public void UploadBytes(string bucket, string fileKey, byte[] localFile) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000";
                string cosPath = fileKey;// "exampleObject"; // 对象键
                byte[] data = localFile; //new byte[1024]; // 二进制数据
                PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, data);
                // 发起上传
                PutObjectResult? result = cosXml?.PutObject(putObjectRequest);
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        // 文件流上传, 从 5.4.24 版本开始支持
        public void PutObjectStream(string bucket, string flieKey, Stream fileStream) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000";
                string key = flieKey;// "exampleobject"; //对象键
                //string srcPath = @"temp-source-file";//本地文件绝对路径
                // 打开只读的文件流对象
                //FileStream fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                // 组装上传请求，其中 offset sendLength 为可选参数
                long offset = 0L;
                long sendLength = fileStream.Length;

                PutObjectRequest request = new PutObjectRequest(bucket, key, fileStream, offset, sendLength);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total) {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult? result = cosXml?.PutObject(request);
                //关闭文件流
                fileStream.Close();
                //打印请求结果
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// DownloadObject demo = new DownloadObject();
        /// 初始化COS服务
        ///    demo.InitCosXml();
        /// demo.TransferDownloadObject().Wait();
        /// </example>
        public async Task TransferDownloadObject(string bucket, string fileKey, string localDir, string localFileName) {
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置高级下载接口的分块阈值为 20MB(默认为20MB), 从5.4.26版本开始支持！
            //transferConfig.DivisionForDownload = 20 * 1024 * 1024;
            // 手动设置高级下载接口的分块大小为 10MB(默认为5MB),设置过小的分块值可能导致频繁重试或下载速度不合预期
            //transferConfig.SliceSizeForDownload = 10 * 1024 * 1024;

            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            // var AppId = EncrypterHelper.Decrypt(APPID);
            // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000"; //存储桶，格式：BucketName-APPID
            string cosPath = fileKey;// "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
                                     //string localDir = Path.GetTempPath();//本地文件夹
                                     //string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                                     // 下载对象
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, cosPath,
                localDir, localFileName);

            //开启断点续传，当本地存在未下载完成文件时，追加下载到文件末尾
            //本地文件已存在部分不符合本次下载的内容，可能导致下载失败，请删除文件重试
            //downloadTask.SetResumableDownload(true);

            // 手动设置高级下载接口的并发数 (默认为5), 从5.4.26版本开始支持！
            // downloadTask.SetMaxTasks(10);

            //设置进度打印回调函数
            downloadTask.progressCallback = delegate (long completed, long total) {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            try {
                COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);
                Console.WriteLine(result.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 批量下载文件
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="localDir"></param>
        /// <param name="localFiles"></param>
        public void BatchDownload(string bucket, string localDir, Dictionary<string, string> localFiles) {
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            // var AppId = EncrypterHelper.Decrypt(APPID);
            // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000";
            // string localDir = System.IO.Path.GetTempPath();//本地文件夹

            foreach(var fileKey in localFiles.Keys) {
                // 下载对象
                string cosPath = fileKey;// "exampleobject" + i; //对象在存储桶中的位置标识符，即称对象键
                string localFileName = localFiles[fileKey]; // "my-local-temp-file"; //指定本地保存的文件名
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, cosPath,
                    localDir, localFileName);
                transferManager.DownloadAsync(downloadTask).Wait();
            }
        }

        /// <summary>
        /// 下载文件夹
        /// </summary>
        public void GetObjectsFromFolder(string bucket, string folder, string localDir) {
            //需要通过组合 “指定前缀列出” 和 “遍历列出的对象key做下载” 两种操作，实现类似文件夹下载的操作
            //下面的操作，把对象列出到列表里，然后异步下载列表中的对象
            string? nextMarker = null;
            List<string> downloadList = new List<string>();
            // bucket = $"{bucket}-{APPID}";// "examplebucket-1250000000";
            string prefix = folder; // "folder1/"; //指定前缀
            // 循环请求直到没有下一页数据
            do {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                GetBucketRequest listRequest = new GetBucketRequest(bucket);
                //获取 folder1/ 下的所有对象以及子目录
                listRequest.SetPrefix(prefix);
                listRequest.SetMarker(nextMarker);

                //执行列出对象请求
                GetBucketResult? listResult = cosXml?.GetBucket(listRequest);
                ListBucket? info = listResult?.listBucket;
                // 对象列表
                List<ListBucket.Contents>? objects = info?.contentsList;
                // 下一页的下标
                nextMarker = info?.nextMarker;
                if(objects != null && objects.Any()) {
                    //对象列表
                    foreach(var content in objects) {
                        downloadList.Add(content.key);
                        Console.WriteLine("adding key:" + content.key);
                    }
                }
            } while(nextMarker != null);
            Console.WriteLine("download list construct done, " + downloadList.Count + " objects added");

            TransferConfig transferConfig = new TransferConfig();
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            //string localDir = System.IO.Path.GetTempPath(); //本地文件夹
            List<Task> taskList = new List<Task>();
            for(int i = 0; i < downloadList.Count; i++) {
                // 遍历待下载列表，下载内容写到 filename_i 文件中
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, downloadList[i],
                    localDir, "filename_" + i.ToString());
                // 异步下载, 加入task队列
                Task task = transferManager.DownloadAsync(downloadTask);
                taskList.Add(task);
            }
            Console.WriteLine("download tasks submitted, total " + taskList.Count + " tasks added");
            //等待TaskList中所有Task结束

            foreach(Task task in taskList) {
                task.Wait();
                Console.WriteLine("download completed");
            }
        }

        /// <summary>
        /// 单链接限速下载
        /// </summary>
        public async void LimitSpeedDownload(string bucket, string fileKey, string localDir, string localFileName, int speedLimit = 8 * 1000 * 1024) {
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            // var AppId = EncrypterHelper.Decrypt(APPID);
            // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000"; //存储桶，格式：BucketName-APPID
            string cosPath = fileKey;// "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
            //string localDir = System.IO.Path.GetTempPath();//本地文件夹
            //string localFileName = "my-local-temp-file"; //指定本地保存的文件名

            GetObjectRequest request = new GetObjectRequest(bucket, cosPath, localDir, localFileName);
            request.LimitTraffic(speedLimit); // 限制为1MB/s

            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
            await transferManager.DownloadAsync(downloadTask);
        }

        /// <summary>
        /// 单对象简单下载
        /// </summary>
        public void GetObject(string bucket, string fileKey, string localDir, string localFileName) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // var AppId = EncrypterHelper.Decrypt(APPID);
                // bucket = $"{bucket}-{AppId}";// "examplebucket-1250000000";
                string key = fileKey;// "exampleobject"; //对象键
                //string localDir = Path.GetTempPath();//本地文件夹
                //string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
                request.SetCosProgressCallback(delegate (long completed, long total) {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult? result = cosXml?.GetObject(request);
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        /// <summary>
        /// 下载到内存
        /// </summary>
        public void DownloadToMemory(string bucket, string fileKey) {
            try {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // bucket = $"{bucket}-{APPID}";// "examplebucket-1250000000";
                string key = fileKey;// "exampleobject"; //对象键

                GetObjectBytesRequest request = new GetObjectBytesRequest(bucket, key);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total) {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectBytesResult? result = cosXml?.GetObject(request);
                //获取内容到 byte 数组中
                byte[]? content = result?.content;
                //请求成功
                Console.WriteLine(result?.GetResultInfo());
            }
            catch(COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch(COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }
    }
}
