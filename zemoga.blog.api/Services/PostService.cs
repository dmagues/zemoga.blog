using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.DataAccess.Infrastructure;
using zemoga.blog.api.DataAccess.Repositories;
using zemoga.blog.api.Models;

namespace zemoga.blog.api.Services
{
    public interface IPostService
    {
        Task<Post> GetById(int id);
        Task<List<Post>> GetByUser(int userId);
        bool ValidateApprove(Post post);
        void Approve(Post post);
    }
    public class PostService : IPostService
    {
        private RepositoryWrapper _repository;

        public PostService(RepositoryWrapper repository)
        {
            this._repository = repository;
        }

        public async Task<Post> GetById(int id)
        {
            var post = await this._repository.Post.GetByCondition(p => p.PostId == id);
            return post.FirstOrDefault();
        }

        public void Approve(Post post)
        {
            if (post == null)
            {
                throw new ArgumentException("Post doesn't exists");
            }

            post.Status = PostStatus.Published;
            this._repository.Post.Update(post);
        }

        public async Task<List<Post>> GetByUser(int userId)
        {
            var posts = await this._repository.Post.GetByCondition(p => p.AuthorId == userId);
            return posts.ToList();
        }

        public bool ValidateApprove(Post post)
        {
            if (post == null)
            {
                throw new ArgumentException("Post doesn't exists");
            }

            
            if(post.Status != PostStatus.Pending)
            {
                return false;
            }
            
            return true;
        }
    }
}
