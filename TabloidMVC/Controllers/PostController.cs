using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }
        public IActionResult MyPosts()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myPosts = _postRepository.GetPostsById(int.Parse(id));
            return View(myPosts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }
        public ActionResult Edit(int id)
        {
            List<Category> categoryOptions = _categoryRepository.GetAll();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Post post = _postRepository.GetUserPostById(id, int.Parse(userId));

            PostCreateViewModel vm = new PostCreateViewModel()
            {
                Post = post,
                CategoryOptions = categoryOptions
            };


            if (post == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _postRepository.UpdatePost(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch (Exception ex)
            {
                vm.CategoryOptions = _categoryRepository.GetAll();

                return View(vm);
            }
        }
        public ActionResult Delete(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Post post = _postRepository.GetUserPostById(id, int.Parse(userId));
           
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: DogsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
              
                _postRepository.DeletePost(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }


        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
