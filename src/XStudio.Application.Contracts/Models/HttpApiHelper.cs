using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;
using System.Net.Http.Json;
using XStudio.Models.Responses;
using XStudio.Models.Requests;

namespace XStudio.Models {
    public class HttpApiHelper {
        private static HttpClient? httpClient = null;
        private static string? _accessToken = null;
        public bool IsAuthorization { get; private set; } = false;

        public HttpApiHelper() {
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.Timeout = new TimeSpan(0, 0, 30);
        }

        public async Task ChangeBaseAddress(string baseAddress, int timeout = 30) {
            if (baseAddress == null) {
                throw new ArgumentException("Base address cannot be null.");
            }
            Uri uri;
            try {
                uri = new Uri(baseAddress);
            }
            catch (UriFormatException ex) {
                throw new ArgumentException("Base address is not a valid URI.", ex);
            }
            if (httpClient == null || httpClient.BaseAddress == null ||!httpClient.BaseAddress.Equals(uri)) {
                if (httpClient != null) {
                    httpClient.Dispose();
                    await Task.Delay(50);
                }
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.Timeout = TimeSpan.FromSeconds(timeout);
                httpClient.BaseAddress = uri;
                if (IsAuthorization) {
                    SetAuthorizationHeader(_accessToken);
                }
            }
        }
        /// <summary>
        /// 清除头部信息
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void ClearefaultRequestHeaders() {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            httpClient.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddDefaultRequestHeader(string name, string? value) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(name, value);
        }

        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddDefaultRequestHeader(string name, IEnumerable<string?> values) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(name, values);
        }

        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="headers"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddDefaultRequestHeaders(Dictionary<string, string?> headers) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            foreach (var header in headers) {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="headers"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddDefaultRequestHeaders(Dictionary<string, IEnumerable<string?>> headers) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            foreach (var header in headers) {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        /// <summary>
        /// 设置授权头部信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SetAuthorizationHeader(string? accessToken) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            if (string.IsNullOrEmpty(accessToken)) {
                IsAuthorization = false;
                httpClient.DefaultRequestHeaders.Authorization = null;
                return;
            }
            IsAuthorization = true;
            _accessToken = accessToken;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        

        public async Task<T?> LoginAsync<T>(string endpoint, UserData data) {
            if (httpClient == null) { 
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode) {
                return await ReadAsAsync<T>(response.Content);
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }



        public async Task<TokenRes?> TokenAsync(string endpoint, TokenReq data) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", data.GrantType),
                new KeyValuePair<string, string>("client_id", data.ClientId),
                //new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("scope", data.Scope),
                new KeyValuePair<string, string>("username", data.UserName),
                new KeyValuePair<string, string>("password", data.Password),
            });
            HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
            if (response.IsSuccessStatusCode) {
                return await ReadAsAsync<TokenRes>(response.Content);
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<T?> ReadAsAsync<T>(HttpContent content) {
            string contentString = await content.ReadAsStringAsync();//.ReadAsAsync<T>();
            var result = JsonConvert.DeserializeObject<T>(contentString);
            return result;
        }

        public async Task<T?> ExecuteAsync<T>(Func<Task<T>> func) {
            try {
                if (!IsAuthorization) {
                    // 未授权，尝试获取token
                    throw new InvalidOperationException("Authorization is required.");
                }
                // 尝试将内容读取为指定类型
                return await func();
            }
            catch (HttpRequestException e) {
                // 处理请求异常
                Console.WriteLine($"请求错误: {e.Message}");
                Log.Error(e.Message, e);
                return default;
                //throw new HttpRequestException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            catch (InvalidOperationException e) {
                // 处理无效操作异常，例如没有正确的serialization设置
                Console.WriteLine($"无效操作错误: {e.Message}");
                Log.Error(e.Message, e);
                return default;
                //throw new InvalidOperationException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            catch (Exception e) {
                // 处理其他未知异常
                Console.WriteLine($"发生错误: {e.Message}");
                Log.Error(e.Message, e);
                return default;
                //throw new InvalidOperationException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            finally {

            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> GetAsync<T>(string endpoint) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> GetListAsync<T>(string endpoint, PagedAndSortedResultRequestDto input) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }
                return default;
                // throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> PostAsync<T>(string endpoint, T data) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> PostManyAsync<T>(string endpoint, T data, string mediaType = "application/json") {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, mediaType);
                // 发送 DELETE 请求并包含请求体
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint) {
                    Content = content
                };
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> PutAsync<T>(string endpoint, T data) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.PutAsJsonAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error putting data to API: {response.ReasonPhrase}");
            });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> DeleteAsync<T>(string endpoint) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
            });
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> DeleteManyAsync<T>(string endpoint, List<Guid> ids, string mediaType = "application/json") {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                var content = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, mediaType);
                // 发送 DELETE 请求并包含请求体
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, endpoint) {
                    Content = content
                };
                HttpResponseMessage? response = await SendAsync(request);
                response?.EnsureSuccessStatusCode();
                if (response != null && response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error deleting data from API: {response?.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage?> SendAsync(HttpRequestMessage request) {

            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => await httpClient.SendAsync(request));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage?> GetResponseAsync(string endpoint) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => await httpClient.GetAsync(endpoint));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage?> PostResponseAsync(string endpoint, HttpContent content) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => await httpClient.PostAsync(endpoint, content));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage?> PutResponseAsync(string endpoint, HttpContent content) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => await httpClient.PutAsync(endpoint, content));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage?> DeleteResponseAsync(string endpoint) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => await httpClient.DeleteAsync(endpoint));
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage?> SendResponseAsync(HttpRequestMessage request) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => await httpClient.SendAsync(request));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> GetResponseAsync<T>(string endpoint) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> PostResponseAsync<T>(string endpoint, HttpContent content) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> PutResponseAsync<T>(string endpoint, HttpContent content) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error putting data to API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> DeleteResponseAsync<T>(string endpoint) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> SendResponseAsync<T>(HttpRequestMessage request) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> SendResponseAsync<T>(HttpRequestMessage request, HttpCompletionOption completionOption) {
            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.SendAsync(request, completionOption);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
            });
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="completionOption"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T?> SendResponseAsync<T>(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {

            if (httpClient == null) {
                throw new InvalidOperationException("HttpClient is not initialized.");
            }
            return await ExecuteAsync(async () => {
                HttpResponseMessage response = await httpClient.SendAsync(request, completionOption, cancellationToken);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) {
                    return await ReadAsAsync<T>(response.Content);
                }

                throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
            });
        }
    }
}
