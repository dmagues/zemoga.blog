using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using zemoga.blog.webui.Business.DTO;
using zemoga.blog.webui.Models;
using zemoga.blog.webui.Runtime;

namespace zemoga.blog.webui.Business.REST
{
    public interface IPostRestApiClient
    {
        string AuthToken { get; set; }
        Task<List<PostModel>> GetAll();
        Task Create(PostModel model);
        Task<List<PostModel>> GetOwner();
        Task<List<PostModel>> GetPending();
        Task<PostModel> GetById(int id);
        Task Approve(int id);
        Task Reject(int id);
    }
    public class PostRestApiClient :  RestApiClientBase, IPostRestApiClient
    {
        
        public PostRestApiClient(IOptions<AppSettings> appSettings):base(appSettings)
        {
            
        }

        public async Task Create(PostModel model)
        {
            var postDto = new PostDTO()
            {
                PostId = model.PostId,
                AuthorId = model.AuthorId,
                Title = model.Title,
                Content = model.Content,
                PublishedDate = model.PublishedDate,
                Status = model.Status
            };

            var json = JsonConvert.SerializeObject(postDto);            
            var response = await Post("posts", json);
            response.EnsureSuccessStatusCode();            
        }

        public async Task<List<PostModel>> GetAll()
        {
            var response = await Get($"posts/?status={(int)PostStatus.Published}");
            response.EnsureSuccessStatusCode();
            var postsDtoString = await response.Content.ReadAsStringAsync();
            var postsDto = JsonConvert.DeserializeObject<List<PostDTO>>(postsDtoString);
            return postsDto.Select(dto => new PostModel()
            {
                PostId = dto.PostId,
                AuthorId = dto.AuthorId,
                Author = dto.Author,
                Title = dto.Title,
                Content = dto.Content,
                PublishedDate = dto.PublishedDate,
                Status = dto.Status
            }).ToList();
        }

        public async Task<PostModel> GetById(int id)
        {
            var response = await Get($"posts/{id}");
            response.EnsureSuccessStatusCode();
            var postDtoString = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<PostDTO>(postDtoString);
            return new PostModel()
            {
                PostId = dto.PostId,
                AuthorId = dto.AuthorId,
                Author = dto.Author,
                Title = dto.Title,
                Content = dto.Content,
                PublishedDate = dto.PublishedDate,
                Status = dto.Status
            };
        }

        public async Task<List<PostModel>> GetOwner()
        {
            var response = await Get("posts/me");
            response.EnsureSuccessStatusCode();
            var postsDtoString = await response.Content.ReadAsStringAsync();
            var postsDto = JsonConvert.DeserializeObject<List<PostDTO>>(postsDtoString);
            return postsDto.Select(dto => new PostModel()
            {
                PostId = dto.PostId,
                AuthorId = dto.AuthorId,
                Author = dto.Author,
                Title = dto.Title,
                Content = dto.Content,
                PublishedDate = dto.PublishedDate,
                Status = dto.Status
            }).ToList();
        }

        public async Task<List<PostModel>> GetPending()
        {
            var response = await Get($"posts/?status={(int)PostStatus.Pending}");
            response.EnsureSuccessStatusCode();
            var postsDtoString = await response.Content.ReadAsStringAsync();
            var postsDto = JsonConvert.DeserializeObject<List<PostDTO>>(postsDtoString);
            return postsDto.Select(dto => new PostModel()
            {
                PostId = dto.PostId,
                AuthorId = dto.AuthorId,
                Author = dto.Author,
                Title = dto.Title,
                Content = dto.Content,
                PublishedDate = dto.PublishedDate,
                Status = dto.Status
            }).ToList();
        }

        public async Task Approve(int id)
        {
            var response = await Put($"posts/{id}/approve", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task Reject(int id)
        {
            var response = await Put($"posts/{id}/reject", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
