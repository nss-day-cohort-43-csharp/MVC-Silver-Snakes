using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        // GET: CommentController
        public ActionResult Index(int id)
        {
            //Must use a ViewModel because we need info on the Post and Comments
            var vm = CreateIndexVM(id);

            return View(vm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            var vm = new CommentFormViewModel();

            Post post = _postRepository.GetPublishedPostById(id);

            vm.Post = post;

            return View(vm);
        }

        // POST: CommentController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //Pass in PostId and Comment object from create form
        public ActionResult Create(int id, CommentFormViewModel vm)
        {
            try
            {
                vm.Comment.CreateDateTime = DateAndTime.Now;
                vm.Comment.PostId = id;
                vm.Comment.UserProfileId = GetCurrentUserProfileId();

                _commentRepository.Add(vm.Comment);

                return RedirectToAction("Index", "Comment", new { Id = id });
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            int userId = GetCurrentUserProfileId();
            Post post = _postRepository.GetUserPostById(id, userId);
            Comment comment = _commentRepository.GetCommentById(id, userId);

            CommentFormViewModel vm = new CommentFormViewModel()
            {
                Post = post,
                Comment = comment
            };

            if (comment == null)
            {
                return NotFound();
            }
            return View(vm);
        }
        //public ActionResult Edit(int id)
        //{
        //    int userId = GetCurrentUserProfileId();
        //    Comment comment = _commentRepository.GetCommentById(id, userId);

        //    if (comment == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(comment);
        //}

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                int userId = GetCurrentUserProfileId();
                _commentRepository.UpdateComment(comment);


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
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

        // Get the Post's Id and redirect from Comment list to the post
        public IActionResult Post(int id)
        {
            return RedirectToAction("Details", "Post", new { Id = id });
        }


        private CommentIndexViewModel CreateIndexVM(int id)
        {
            var vm = new CommentIndexViewModel();

            Post post = _postRepository.GetPublishedPostById(id);
            List<Comment> comments = _commentRepository.getAllByPost(id);

            vm.Post = post;
            vm.Comments = comments;

            return vm;
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
