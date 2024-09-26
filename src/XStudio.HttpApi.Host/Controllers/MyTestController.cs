using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Volo.Abp.OpenIddict;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using XStudio.Helpers;
using XStudio.Encrypter;
using Nacos.V2;
using System.Net.Http;
using Nacos.V2.Utils;
using System.Text;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.EventBus.Distributed;
using XStudio.Common.Kafka;
using Volo.Abp.Uow;

namespace XStudio.Controllers
{
    [Route("api/xstudio/v{version:apiVersion}/[controller]")]
    [ApiVersion(2.0)]
    [ApiController]
    public class MyTestController : AbpController
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly INacosNamingService _nnsvc;
        private readonly INacosConfigService _ncsvc;
        private readonly IDistributedCache<object> _distributedCache;
        private readonly IDistributedEventBus _eventBus;
        private KafkaConsumerEventHandler<object, object> _kafkaConsumerEventHandler;
        public MyTestController(IOpenIddictApplicationManager applicationManager,
                                IOpenIddictScopeManager scopeManager,
                                INacosNamingService nnsvc,
                                INacosConfigService ncsvc,
                                IDistributedCache<object> distributedCache,
                                IDistributedEventBus eventBus,
                                IUnitOfWorkManager unitOfWorkManager)
        {
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _nnsvc = nnsvc;
            _ncsvc = ncsvc;
            _distributedCache = distributedCache;
            _eventBus = eventBus;
            _kafkaConsumerEventHandler = new KafkaConsumerEventHandler<object, object>(unitOfWorkManager);
            _kafkaConsumerEventHandler.OnHandleEventAction = async (MessagePackage<object> data) =>
            {
                Console.WriteLine(data?.ToJson());
                await Task.Delay(1000);
                return "消息处理完成";
            };
            _eventBus.Subscribe(_kafkaConsumerEventHandler);
        }


        [HttpPost("encrypt")]
        public IActionResult Encrypt(EncryptDto plain)
        {
            return Ok(EncrypterHelper.Encrypt(plain.PlainText));
        }

        [HttpPost("decrypt")]
        public IActionResult Decrypt(DecryptDto encrypted)
        {
            return Ok(EncrypterHelper.Decrypt(encrypted.EncryptedText));
        }

        // GET api/values/test
        [HttpGet("TestAsync")]
        public async Task<ActionResult<string>> TestAsync()
        {
            var instance = await _nnsvc.SelectOneHealthyInstance("XStudio", "DEFAULT_GROUP");
            if (instance == null)
            {
                return $"没有在Nacos中发现服务 XStudio";
            }
            var isHttps = HttpContext.Request.IsHttps; // 检查是否是通过 HTTPS 访问
            var host = $"{instance.Ip}:{(isHttps ? instance.Metadata["HttpsPort"] : instance.Port)}";
            var baseUrl = isHttps ? $"https://{host}" : $"http://{host}";

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return "empty";
            }
            string path = $"api/xstudio/v1/project/list";
            var url = $"{baseUrl.TrimEnd('/')}/{path}";
            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    var page = new
                    {
                        maxResultCount = 1,
                        skipCount = 0,
                        sorting = "id"
                    };
                    using HttpContent httpContent = new StringContent(page.ToJsonString(), Encoding.UTF8);
                    string contentType = "application/json";
                    if (contentType != null)
                    {
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    }
                    try
                    {
                        var result = await client.PostAsync(url, new StringContent(page.ToJsonString()));
                        return await result.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {

                        return $"{ex.Message}: \r\n {ex.StackTrace}";
                    }
                }
            }
        }

        [HttpPost("AddCache")]
        public async Task SetCacheAsync(string key, object value)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(3)
            };
            await _distributedCache.SetAsync(key, value.ToJson(), options);
        }

        [HttpGet("getCache")]
        public async Task<object?> GetCacheAsync(string key)
        {
            object? obj = await _distributedCache.GetAsync(key);
            return obj;
        }

        [HttpPost("sendKafka")]
        public async Task<ActionResult> SetKafkaAsync(string Topic, object value)
        {
            MessagePackage<object> package = new MessagePackage<object>(value)
            {
                Topic = Topic,
                Key = "AAAAA",
                Title = "Test"
            };
            await _eventBus.PublishAsync(package);
            return Ok(package);
        }

        [HttpGet("getKafka")]
        public async Task<ActionResult> GetKafkaAsync(string Topic)
        {


            await Task.CompletedTask;
            return Ok("成功");
        }
    }
}
