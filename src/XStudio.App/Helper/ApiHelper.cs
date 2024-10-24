using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Abp.Application.Services.Dto;
using XStudio.App.Models.Data;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Client.Http;

namespace XStudio.App.Helper {
    public class ApiHelper {
        private static readonly HttpClient httpClient = new HttpClient();

        public ApiHelper(string? baseAddress) {
            if(baseAddress == null)
            {
                throw new ArgumentException("Base address cannot be null.");
            }
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func) {
            try {
                // 尝试将内容读取为指定类型
                return await func();
            }
            catch (HttpRequestException e) {
                // 处理请求异常
                Console.WriteLine($"请求错误: {e.Message}");
                throw new HttpRequestException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            catch (InvalidOperationException e) {
                // 处理无效操作异常，例如没有正确的serialization设置
                Console.WriteLine($"无效操作错误: {e.Message}");
                throw new InvalidOperationException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
            catch (Exception e) {
                // 处理其他未知异常
                Console.WriteLine($"发生错误: {e.Message}");
                throw new InvalidOperationException($"{e.Message}", e); // 可重新抛出异常或进行其他处理
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> GetAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> GetListAsync<T>(string endpoint, PagedAndSortedResultRequestDto input) {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> PostAsync<T>(string endpoint, T data) {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, data);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> PostManyAsync<T>(string endpoint, T data) {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            // 发送 DELETE 请求并包含请求体
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint) {
                Content = content
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> PutAsync<T>(string endpoint, T data) {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(endpoint, data);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error putting data to API: {response.ReasonPhrase}");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> DeleteAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> DeleteManyAsync<T>(string endpoint, List<Guid> ids) { 
            var content = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json");
            // 发送 DELETE 请求并包含请求体
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, endpoint) {
                Content = content
            };
            HttpResponseMessage response = await SendAsync(request);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
           
            return await ExecuteAsync(async () =>  await httpClient.SendAsync(request));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetResponseAsync(string endpoint) {
            return await ExecuteAsync(async () => await httpClient.GetAsync(endpoint));
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostResponseAsync(string endpoint, HttpContent content) {
            return await ExecuteAsync(async () => await httpClient.PostAsync(endpoint, content));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutResponseAsync(string endpoint, HttpContent content) {
            return await ExecuteAsync(async () => await httpClient.PutAsync(endpoint, content));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteResponseAsync(string endpoint) {
            return await ExecuteAsync(async () => await httpClient.DeleteAsync(endpoint));
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendResponseAsync(HttpRequestMessage request) {
            return await ExecuteAsync(async () => await httpClient.SendAsync(request));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> GetResponseAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> PostResponseAsync<T>(string endpoint, HttpContent content) {
            HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> PutResponseAsync<T>(string endpoint, HttpContent content) {
            HttpResponseMessage response = await httpClient.PutAsync(endpoint, content);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error putting data to API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> DeleteResponseAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> SendResponseAsync<T>(HttpRequestMessage request) {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="completionOption"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> SendResponseAsync<T>(HttpRequestMessage request, HttpCompletionOption completionOption) {
            HttpResponseMessage response = await httpClient.SendAsync(request, completionOption);

            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
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
        public async Task<T> SendResponseAsync<T>(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
            HttpResponseMessage response = await httpClient.SendAsync(request, completionOption, cancellationToken);
            if (response.IsSuccessStatusCode) {
                return await ExecuteAsync(response.Content.ReadAsAsync<T>);
            }

            throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
        }
    }
}
