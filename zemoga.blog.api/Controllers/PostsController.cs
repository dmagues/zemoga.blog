using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zemoga.blog.api.Business.DTO;
using zemoga.blog.api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zemoga.blog.api.Controllers
{
    [Route("api/posts")]
    [Authorize]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostService _postService;
        private ILogger<PostsController> _logger;

        private int UserId => int.Parse(HttpContext.User.Claims.FirstOrDefault(
             c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");

        private string UserName => HttpContext.User.Identity.Name;

        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            this._postService = postService;
            this._logger = logger;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Writer")]
        public async Task<IEnumerable<PostDTO>> GetByUser()
        {
            return await this._postService.GetByUser(UserId);
            
        }
        // GET: api/<PostsController>
        [HttpGet]        
        public async Task<IEnumerable<PostDTO>> Get()
        {
            return await this._postService.Get();
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]        
        public async Task<PostDTO> Get(int id)
        {
            return await this._postService.GetById(id);
        }

        [HttpGet("{id}/comments")]
        public async Task<List<CommentDTO>> GetComment(int id)
        {
            return await this._postService.GetCommentsByPostId(id);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> PostComment(int id, CommentDTO comment)
        {
            try
            {
                comment.AuthorId = UserId; ;
                await this._postService.CreateComment(id, comment);
                return Ok();
            }catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, string.Format("Error in {0} ", nameof(PostComment)));
                return StatusCode(500, string.Format("Error in {0}", nameof(PostComment)));
            }
        }

        [HttpDelete("{id}/comments/{cid}")]
        public void DeleteComment(int cid)
        {

        }

        // POST api/<PostsController>
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Post([FromBody] PostDTO value)
        {
            ;
            try
            {
                value.AuthorId = UserId;
                await this._postService.Create(value);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, string.Format("Error in {0} ", nameof(Post)));
                return StatusCode(500, string.Format("Error in {0} ", nameof(Post)));
            }
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Put(int id, [FromBody] PostDTO value)
        {
            

            try
            {
                value.AuthorId = UserId;
                await this._postService.Update(id, value);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }catch(Exception ex)
            {
                this._logger.LogError(ex, string.Format("Error in {0} ", nameof(Put)));
                return StatusCode(500, string.Format("Error in {0} ", nameof(Put)));
            }
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this._postService.Delete(id);
                return StatusCode(204);
            }catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, string.Format("Error in {0} ", nameof(Delete)));
                return StatusCode(500, string.Format("Error in {0} ", nameof(Delete)));
            }


        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> PutApprove(int id)
        {
            try
            {
                await this._postService.Approve(id);
                return Ok();

            }catch(ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }            
            catch (Exception ex)
            {
                this._logger.LogError(ex, string.Format("Error in {0} ", nameof(PutApprove)));
                return StatusCode(500, string.Format("Error in {0} ", nameof(PutApprove)));
            }            
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> PutReject(int id)
        {
            try
            {
                await this._postService.Reject(id);
                return Ok();

            }
            catch (ArgumentException argEx)
            {
                return NotFound(argEx.Message);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, string.Format("Error in {0} ", nameof(PutReject)));
                return StatusCode(500, string.Format("Error in {0} ", nameof(PutReject)));
            }
        }
    }
}
