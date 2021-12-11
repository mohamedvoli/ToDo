using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;
using TodoList.Models.Repos;
using TodoList.Models.ViewModels;

namespace TodoList.Controllers
{
    public class TodoTasksController : Controller
    {
        private ITodoRepo<TodoTask> _TaskRepo;
        private ITodoRepo<Category> _CategoryRepo;
        private UserManager<ApplicationUser> _UserManager;
        public TodoTasksController(ITodoRepo<TodoTask> TaskRepo,
            UserManager<ApplicationUser> UserManager,
            ITodoRepo<Category> CategoryRepo)
        {
            _TaskRepo = TaskRepo;
            _UserManager = UserManager;
            _CategoryRepo = CategoryRepo;
        }

        // GET: TodoTasksController
        [Authorize]
        public ActionResult Index(string SearchingTerm)
        {
            var UserId = _UserManager.GetUserId(User);
            List<TodoTask> AllTasks = _TaskRepo.List(UserId);
            if (!string.IsNullOrEmpty(SearchingTerm))
            {
                AllTasks = _TaskRepo.Search(SearchingTerm, UserId);
            }
            return View(AllTasks);
        }

        // GET: TodoTasksController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TodoTasksController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View(SetTheModelToGetMethod());
        }

        // POST: TodoTasksController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(TaskCategoryVm model)
        {

            var UserId = _UserManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.CategoryId == -1)
                    {
                        ViewData["Message"] = "Please select a category!"; 
                        return View(SetTheModelToGetMethod());
                    }
                    else
                    {
                        var category = _CategoryRepo.Find(model.CategoryId);
                        var User = await _UserManager.FindByIdAsync(UserId);
                        TodoTask ValidModel = new TodoTask
                        {
                            Title = model.Title,
                            Description = model.Description,
                            TimeStamp = DateTime.Now,
                            IsDone = model.IsDone,
                            ParentCategory = category,
                            User = User
                        };
                        _TaskRepo.Add(ValidModel);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields!");
                return View(FillInSelectList(UserId));
            }
        }

        // GET: TodoTasksController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TodoTasksController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: TodoTasksController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TodoTasksController/Delete/5
        [Authorize]
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
        [Authorize]
        public List<Category> FillInSelectList(string UserId)
        {
            var AllCategories = _CategoryRepo.List(UserId).ToList();
            AllCategories.Insert(0, new Category { CategoryId = -1, Title = "--- Please select a category ---" });
            return AllCategories;
        }
        public TaskCategoryVm SetTheModelToGetMethod()
        {
            var UserId = _UserManager.GetUserId(User);
            var AllCategories = FillInSelectList(UserId);
            TaskCategoryVm model = new TaskCategoryVm
            {
                UserId = UserId,
                Categories = AllCategories
            };
            return model;
        }
    }
}
