using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zemoga.blog.webui.Models;
using zemoga.blog.webui.Business.REST;
using Microsoft.AspNetCore.Authorization;

namespace zemoga.blog.webui.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRestApiClient _postRestApi;

        public PostController(IPostRestApiClient postRestApi, IHttpContextAccessor httpContextAccessor)
        {
            this._postRestApi = postRestApi;
            
            if(httpContextAccessor.HttpContext.User.Claims.Any(c=>c.Type == "BasicToken"))
            {
                this._postRestApi.AuthToken = httpContextAccessor.HttpContext.User.Claims
                    .First(c => c.Type == "BasicToken").Value;
            }
             
        }

        
        // GET: PostController
        public async Task<ActionResult> Index()
        {
            var posts = await this._postRestApi.GetAll();
            return View(posts);
        }

        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> Owner()
        {
            var posts = await this._postRestApi.GetOwner();
            return View(posts);
        }

        [Authorize(Roles = "Editor")]
        public async Task<ActionResult> Pending()
        {
            var posts = await this._postRestApi.GetPending();
            return View(posts);
        }

        // GET: PostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var post = await this._postRestApi.GetById(id);
            return View(post);
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] PostModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this._postRestApi.Create(model);
                    return RedirectToAction(nameof(Owner));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var post = await this._postRestApi.GetById(id);            
            return View(post    );
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [FromForm] PostModel model)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Editor")]
        public async Task<ActionResult> Approve(int id)
        {
            try
            {
                await this._postRestApi.Approve(id);
                return RedirectToAction(nameof(Pending));
            }
            catch
            {
                return RedirectToAction(nameof(Pending)); 
            }
        }

        [Authorize(Roles = "Editor")]
        public async Task<ActionResult> Reject(int id)
        {
            try
            {
                await this._postRestApi.Reject(id);
                return RedirectToAction(nameof(Pending));
            }
            catch
            {
                return RedirectToAction(nameof(Pending));
            }
        }
    }
}
