using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

using zemoga.blog.api.Models;
using zemoga.blog.api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zemoga.blog.api.Controllers
{
    [Route("api/posts")]
    [Authorize]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostService _service;

        public PostsController(IPostService postService)
        {
            this._service = postService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Writer")]
        public async Task<IEnumerable<Post>> GetByUser()
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(
             c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                return await this._service.GetByUser(int.Parse(userId));
            }

            return Array.Empty<Post>();
        }
        // GET: api/<PostsController>
        [HttpGet]        
        public IEnumerable<Post> Get()
        {
            return new Post[] { };
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]        
        public Post Get(int id)
        {
            return null;
        }

        [HttpGet("{id}/comments")]
        public IEnumerable<Comment> GetComment(int id)
        {
            return new Comment[] { };
        }

        [HttpPost("{id}/comments")]
        public void PostComment(int id, Comment comment)
        {
            
        }

        [HttpDelete("{id}/comments/{cid}")]
        public void DeleteComment(int cid)
        {

        }

        // POST api/<PostsController>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public void Post([FromBody] Post value)
        {

        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Writer")]
        public void Put(int id, [FromBody] Post value)
        {
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public void Delete(int id)
        {
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Editor")]
        public async Task PutApproveAsync(int id)
        {
            try
            {
                var post = await this._service.GetById(id);
                if (this._service.ValidateApprove(post))
                {
                    BadRequest(String.Format("Post {0} can't be apporved. Invalid status!", id));
                }

                this._service.Approve(post);

            }catch(ArgumentException argEx)
            {
                BadRequest(argEx.Message);
            }catch(Exception ex)
            {
                StatusCode(500, ex.Message);
            }            
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Editor")]
        public void PutReject(int id)
        {
        }
    }
}
