using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Models;
using TodoList.Models.Repos;

namespace TodoList.Controllers
{
    public class CategoryController : Controller
    {
        private ITodoRepo<Category> _categoryRepo;
        private UserManager<ApplicationUser> _userManager;
        public CategoryController(ITodoRepo<Category> categoryRepo, UserManager<ApplicationUser> userManager)
        {
          _categoryRepo = categoryRepo;
          _userManager = userManager;
        }
        // GET: CategoryController
        [Authorize]
        [HttpGet]
        public ActionResult Index(string SearchString)
        {
            var UserId = _userManager.GetUserId(User);
            var AllCategories = _categoryRepo.List(UserId);
            if (!string.IsNullOrEmpty(SearchString))
            {
                AllCategories = _categoryRepo.Search(SearchString, UserId);
            }
            return View(AllCategories);
        }

        // GET: CategoryController/Create
        [Authorize]
        public ActionResult Create()
        {
             ViewData["UserId"] = _userManager.GetUserId(User);
            return View();
        }

        // POST: CategoryController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                _categoryRepo.Add(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var Categroy = _categoryRepo.Find(id);
                return View(Categroy);
            }
            
        }

        // POST: CategoryController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Category category)
        {
            try
            {
                _categoryRepo.Edit(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var Category = _categoryRepo.Find(id);
            return View(Category);
        }

        // POST: CategoryController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Category category)
        {
            try
            {
                _categoryRepo.Delete(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
