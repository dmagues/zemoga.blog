using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using zemoga.blog.webui.Runtime;

namespace zemoga.blog.webui.Business.REST
{
    public interface IRestApiClientBase
    {
        string AuthToken { get; set; }
    }
    public class RestApiClientBase : IRestApiClientBase
    {
        protected readonly AppSettings _appSettings;
        protected readonly Uri _baseUri;
        public string AuthToken { get; set; }
        public RestApiClientBase(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
            this._baseUri = new Uri(this._appSettings.RestApiUrl);
        }

        protected AuthenticationHeaderValue GetAuthorizationHeader()
        {
            return new AuthenticationHeaderValue("Basic", this.AuthToken);
        }

        protected async Task<HttpResponseMessage> Get(string path)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = GetAuthorizationHeader();

            var targetUrl = new Uri(this._baseUri, path);
            var response = await httpClient.GetAsync(targetUrl);
            return response;
        }

        protected async Task<HttpResponseMessage> Post(string path, string json)
        {
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = GetAuthorizationHeader();

            var targetUrl = new Uri(this._baseUri, path);
            var response = await httpClient.PostAsync(targetUrl, data);
            return response;
        }

        protected async Task<HttpResponseMessage> Put(string path, string json)
        {
            StringContent data = null;
            if (!string.IsNullOrEmpty(json))
            {
                data = new StringContent(json, Encoding.UTF8, "application/json");
            }
            

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = GetAuthorizationHeader();

            var targetUrl = new Uri(this._baseUri, path);
            var response = await httpClient.PutAsync(targetUrl, data);
            return response;
        }
    }
}
