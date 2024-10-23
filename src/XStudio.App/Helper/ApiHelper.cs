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

        public async Task<T> GetAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                var data = await response.Content.ReadAsAsync<T>();
                return data;
            }

            throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
        }

        public async Task<T> GetListAsync<T>(string endpoint, PagedAndSortedResultRequestDto input) {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                var data = await response.Content.ReadAsAsync<T>();
                return data;
            }

            throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
        }

        public async Task<T> PostAsync<T>(string endpoint, T data) {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(endpoint, data);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }

        public async Task<T> PutAsync<T>(string endpoint, T data) {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(endpoint, data);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error putting data to API: {response.ReasonPhrase}");
        }

        public async Task<T> DeleteAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
        }   

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetResponseAsync(string endpoint) {
            return await httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostResponseAsync(string endpoint, HttpContent content) {
            return await httpClient.PostAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> PutResponseAsync(string endpoint, HttpContent content) {
            return await httpClient.PutAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteResponseAsync(string endpoint) {
            return await httpClient.DeleteAsync(endpoint);
        }

        public async Task<HttpResponseMessage> SendResponseAsync(HttpRequestMessage request) {
            return await httpClient.SendAsync(request);
        }

        public async Task<T> GetResponseAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                var data = await response.Content.ReadAsAsync<T>();
                return data;
            }

            throw new Exception($"Error retrieving data from API: {response.ReasonPhrase}");
        }

        public async Task<T> PostResponseAsync<T>(string endpoint, HttpContent content) {
            HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error posting data to API: {response.ReasonPhrase}");
        }

        public async Task<T> PutResponseAsync<T>(string endpoint, HttpContent content) {
            HttpResponseMessage response = await httpClient.PutAsync(endpoint, content);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error putting data to API: {response.ReasonPhrase}");
        }

        public async Task<T> DeleteResponseAsync<T>(string endpoint) {
            HttpResponseMessage response = await httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error deleting data from API: {response.ReasonPhrase}");
        }

        public async Task<T> SendResponseAsync<T>(HttpRequestMessage request) {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
        }

        public async Task<T> SendResponseAsync<T>(HttpRequestMessage request, HttpCompletionOption completionOption) {
            HttpResponseMessage response = await httpClient.SendAsync(request, completionOption);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
        }

        public async Task<T> SendResponseAsync<T>(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
            HttpResponseMessage response = await httpClient.SendAsync(request, completionOption, cancellationToken);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new Exception($"Error sending data to API: {response.ReasonPhrase}");
        }
    }
}
