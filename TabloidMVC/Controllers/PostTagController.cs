using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TabloidMVC.Repositories;
using TabloidMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace TabloidMVC.Models
{
    [Authorize]
    public class PostTagController : Controller
    {
        private readonly IPostTagRepository _postTagRepo;
        private readonly ITagRepository _tagRepo;
        private readonly IPostRepository _postRepo;

        public PostTagController(IPostTagRepository postTagRepo, ITagRepository tagRepo, IPostRepository postRepo)
        {
            _postTagRepo = postTagRepo;
            _tagRepo = tagRepo;
            _postRepo = postRepo;
        }

        // GET: PostTagsController
        public ActionResult Index()
        {

            int postId = Int32.Parse(HttpContext.Request.Query["postId"]);

            Post post = _postRepo.GetPublishedPostById(postId);

            if (post == null)
            {
                return NotFound();
            }

            List<PostTag> postTags = _postTagRepo.GetPostTagsbyPostId(postId); ;


            List<Tag> tags = _tagRepo.GetTags();


            foreach (PostTag pTag in postTags)
            {
                int tagId = pTag.TagId;
                tags.RemoveAll(t => t.Id == tagId);
            }

            PostTagIndexViewModel vm = new PostTagIndexViewModel
            {
                Post = post,
                PostTags = postTags, 
                Tags = tags 
            };

            return View(vm);
        }

        // GET: PostTagsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostTagsController/Delete/5
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

        public ActionResult AddToPost(PostTag postTag)
        {
            try
            {
                _postTagRepo.AddTag(postTag);
                return RedirectToAction("Index", "PostTag", new { @postId = postTag.PostId });
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult RemoveFromPost(PostTag postTag)
        {
            try
            {

                PostTag foundPostTag = _postTagRepo.GetPostTagbyPostIdAndTagId(postTag.PostId, postTag.TagId);

                if (foundPostTag != null)
                {
                    _postTagRepo.DeleteTag(foundPostTag.Id);
                }

                return RedirectToAction("Index", "PostTag", new { @postId = postTag.PostId });
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}