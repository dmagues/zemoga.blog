using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using zemoga.blog.webui.Business;

namespace zemoga.blog.webui.Runtime
{
    public class PostRestApiClient
    {
        private AppSettings _appSettings;
        private Uri _baseUri;

        public PostRestApiClient(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
            this._baseUri = new Uri(this._appSettings.RestApiUrl);
        }

        public async Task<List<PostDTO>> GetAll()
        {
            using var httpClient = new HttpClient();           
            var targetUrl = new Uri(this._baseUri, "posts");
            var response = await httpClient.GetAsync(targetUrl);
            response.EnsureSuccessStatusCode();
            var postsString = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<List<PostDTO>>(postsString);
            return posts;
        }
    }
}
