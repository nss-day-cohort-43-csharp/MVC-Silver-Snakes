﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfile;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfile = userProfileRepository;
        }

        //public UserProfile GetUserProfileById(int id)
        //{
        //    return _userProfile.GetUserProfileById(id);
        //}



        // GET: UserProfileController
        public ActionResult Index()
        {
            var userProfiles = _userProfile.GetAll();
            return View(userProfiles);
        }

        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile userProfile = _userProfile.GetUserProfileById(id);
            return View(userProfile);
        }

        // GET: UserProfileController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UserProfileController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserProfileController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UserProfileController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserProfileController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: UserProfileController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateUserProfile(int id)
        {
            UserProfile userprofile = _userProfile.GetUserProfileById(id);
            try
            {
                _userProfile.DeactivateUserProfile(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(userprofile);
            }
        }


    }
}
