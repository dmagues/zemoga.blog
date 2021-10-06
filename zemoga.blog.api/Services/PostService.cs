using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.api.Business.DTO;
using zemoga.blog.api.DataAccess.Repositories;
using zemoga.blog.api.Models;

namespace zemoga.blog.api.Services
{
    public interface IPostService
    {
        Task<PostDTO> GetById(int id);
        Task<List<PostDTO>> GetByUser(int userId);
        Task Approve(int id);
        Task<List<PostDTO>> Get(int? status);
        Task Delete(int id, int userId);
        Task Update(int postId, PostDTO value);
        Task Create(PostDTO value);
        Task<List<CommentDTO>> GetCommentsByPostId(int id);
        Task CreateComment(int postId, CommentDTO comment);
        Task Reject(int id, CommentDTO coment);
    }
    public class PostService : IPostService
    {
        private readonly IRepositoryWrapper _repository;

        public PostService(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        public async Task<PostDTO> GetById(int id)
        {
            var post = await this._repository.Post.GetByConditionWithIncludes(p => p.PostId == id, p => p.Author);
            return post.Select(MapPostDTO()).FirstOrDefault();
        }

        public async Task Approve(int id)
        {
            Post post = await PreCheck(id);
            post.Status = PostStatus.Published;
            this._repository.Post.Update(post);
            await this ._repository.SaveAsync();
        }


        

        public async Task<List<PostDTO>> GetByUser(int userId)
        {
            var posts = await this._repository.Post.GetByConditionWithIncludes(p => p.AuthorId == userId, p => p.Author);
            return posts.Select(MapPostDTO()).ToList();
        }

        
        public async Task<List<PostDTO>> Get(int? status)
        {
            if (status.HasValue) {
                var posts = await this._repository.Post.GetByConditionWithIncludes(p => p.Status == (PostStatus)status, p => p.Author);
                return posts.Select(MapPostDTO()).ToList();
            }
            else
            {
                var posts = await this._repository.Post.GetAllWithIncludes(p => p.Author);
                return posts.Select(MapPostDTO()).ToList();
            }
            
        }

        private static Func<Post, PostDTO> MapPostDTO()
        {
            return p => new PostDTO()
            {
                PostId = p.PostId,
                AuthorId = p.AuthorId,
                Author = p.Author.UserName,
                Title = p.Title,
                Content = p.Content,
                PublishedDate = p.PublishedDate,
                Status = p.Status
            };
        }


        public async Task Delete(int id, int userId)
        {
            var posts = await this._repository.Post.GetByCondition(p => p.PostId == id);
            var postToDelete = posts.FirstOrDefault();


            if (postToDelete == null)
            {
                throw new ArgumentException("Post doesn't exists.");                
            }

            if(postToDelete.AuthorId != userId)
            {
                throw new ApplicationException("Post doesn't belong to the user.");
            }

            this._repository.Post.Delete(postToDelete);
            await this._repository.SaveAsync();



        }

        public async Task Update(int postId, PostDTO value)
        {
            var posts = await this._repository.Post.GetByCondition(p => p.PostId == postId);
            var post = posts.First();

            if (post == null)
            {
                throw new ArgumentException("Post doesn't exists.");
            }

            if (post.AuthorId != value.AuthorId)
            {
                throw new ApplicationException(string.Format("Post {0} doesn't belong to User", postId));
            }
            if (post.Status == PostStatus.Published)
            {
                throw new ApplicationException(string.Format("Post {0} is already published.", post.PostId));
            }

            // TODO: Validate all fields
            post.PublishedDate = value.PublishedDate;
            post.AuthorId = value.AuthorId;
            post.Title = value.Title;
            post.Content = value.Content;
            post.Status = PostStatus.Pending;

            this._repository.Post.Update(post);
            await this._repository.SaveAsync();           

        }

        public async Task Create(PostDTO value)
        {
            //TODO: Validate all fields;
            if (string.IsNullOrEmpty(value.Title))
            {
                throw new ApplicationException("Title is required");
            }

            if (string.IsNullOrEmpty(value.Content))
            {
                throw new ApplicationException("Content is required");
            }

            if (!value.PublishedDate.HasValue)
            {
                throw new ApplicationException("Publish Date is required");
            }

            var post = new Post()
            {
                PublishedDate = value.PublishedDate
                ,AuthorId = value.AuthorId
                ,Title = value.Title
                ,Content = value.Content
                ,Status = PostStatus.Pending
            };            
            
            this._repository.Post.Create(post);
            await this ._repository.SaveAsync();
        }        

        public async Task<List<CommentDTO>> GetCommentsByPostId(int id)
        {
            var comments = await this._repository.Comment.GetByConditionWithIncludes(p => p.PostId == id, p => p.Author);
            return comments.Select(c => new CommentDTO()
            {
                CommentId = c.CommentId,
                PostId = c.PostId,
                AuthorId = c.AuthorId,
                Author = c.Author.UserName,
                Content = c.Content,
                CommentDate = c.CommentDate,
                IsRejected = c.IsRejected
            }).ToList();
        }

        public async Task CreateComment(int postId, CommentDTO value)
        {
            var post = await this._repository.Post.GetByCondition(p => p.PostId == postId);
            if (!post.Any())
            {
                throw new ApplicationException(string.Format("Post {0} doesn't exists.", postId));
            }

            var comment = new Comment()
            {
                PostId = postId,
                AuthorId = value.AuthorId,
                Content = value.Content,
                CommentDate = value.CommentDate
            };
            
            this._repository.Comment.Create(comment);
            await this._repository.SaveAsync();
        }

        public async Task Reject(int id, CommentDTO value)
        {
            Post post = await PreCheck(id);

            post.Status = PostStatus.Rejected;
            var comment = new Comment()
            {
                PostId = id,
                AuthorId = value.AuthorId,
                Content = value.Content,
                CommentDate = value.CommentDate,
                IsRejected = true
            };
            this._repository.Comment.Create(comment);
            this._repository.Post.Update(post);
            await this._repository.SaveAsync();
        }

        private async Task<Post> PreCheck(int id)
        {
            var postList = await this._repository.Post.GetByCondition(p => p.PostId == id);
            if (!postList.Any())
            {
                throw new ArgumentException("Post doesn't exists");
            }

            var post = postList.First();

            if (post.Status != PostStatus.Pending)
            {
                throw new ApplicationException(string.Format("Post {0} can't be rejected. Status is not pending.", id));
            }

            return post;
        }
    }
}
