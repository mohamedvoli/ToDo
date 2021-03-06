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
    [Authorize]
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
        public ActionResult Index(string? SearchingTerm)
        {
            var UserId = _UserManager.GetUserId(User);
            List<TodoTask> AllTasks = _TaskRepo.List(UserId);
            if (!string.IsNullOrEmpty(SearchingTerm))
            {
                AllTasks = _TaskRepo.Search(SearchingTerm, UserId);
            }
            return View(AllTasks);
        }
        // GET : TodoTaskController/FinishedTasks
        public ActionResult FinishedTasks(string? SearchingTerm)
        {
            var UserId = _UserManager.GetUserId(User);
            List<TodoTask> AllTasks = _TaskRepo.ListFinishedTasks(UserId);
            if (!string.IsNullOrEmpty(SearchingTerm))
            {
                AllTasks = _TaskRepo.SearchFinishedTasks(SearchingTerm, UserId);
            }
            return View(AllTasks);
        }
        // GET: TodoTasksController/Details/5
        public ActionResult Details(int id)
        {
            var ATask = _TaskRepo.Find(id);
            return View(ATask);
        }

        // GET: TodoTasksController/Create
        public ActionResult Create()
        {
            var AllCategories = FillInSelectList(_UserManager.GetUserId(User));
            CreateTaskVm model = new CreateTaskVm
            {
                Categories = AllCategories
            };
            return View(model);
        }

        // POST: TodoTasksController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CreateTaskVm model)
        {

            var UserId = _UserManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                if (model.CategoryId == -1)
                {
                    ModelState.AddModelError("CategoryId","Please Select a category!");
                    model.Categories = FillInSelectList(UserId);
                    return View(model);
                }
                else
                {
                    try
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
                    catch
                    {
                        return View();
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields!");
                return View(model);
            }
        }

        // GET: TodoTasksController/Edit/5
        public ActionResult Edit(int id)
        {
            var ATask = _TaskRepo.Find(id);
            var Categories = _CategoryRepo.List(_UserManager.GetUserId(User));
            EditTaskVm model = new EditTaskVm()
            {
                TaskId = ATask.TodoTaskId,
                Title = ATask.Title,
                TimeStamp = ATask.TimeStamp,
                Description = ATask.Description,
                IsDone = ATask.IsDone,
                Categories = Categories,
                CategoryId = ATask.CategoryId,
                SelectedCategory = ATask.ParentCategory
            };
            return View(model);
        }

        // POST: TodoTasksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTaskVm model)
        {
            var UserId = _UserManager.GetUserId(User); 
            if (ModelState.IsValid)
            {
                var UserIdOfSelectedCategory = _CategoryRepo.Find(model.CategoryId).UserId;
                var CurrentUserId = _UserManager.GetUserId(User);
                bool CompareResult = string.Equals(UserIdOfSelectedCategory, CurrentUserId);
                if (!CompareResult)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    try
                    {
                        TodoTask ATask = new TodoTask()
                        {
                            UserId = UserId,
                            TodoTaskId = model.TaskId,
                            Title = model.Title,
                            Description = model.Description,
                            IsDone = model.IsDone,
                            CategoryId = model.CategoryId,
                            TimeStamp = model.TimeStamp
                        };
                        _TaskRepo.Edit(ATask);
                        return RedirectToAction(nameof(Index));
                    }
                    catch
                    {
                        return NotFound();
                    }
                }
            }
            else
            {
                    return NotFound();
                
            } 
        }

        // GET: TodoTasksController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var ATask = _TaskRepo.Find(id);
            return View(ATask);
        }

        // POST: TodoTasksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(TodoTask model)
        {
            try
            {
                _TaskRepo.Delete(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public List<Category> FillInSelectList(string UserId)
        {
            var AllCategories = _CategoryRepo.List(UserId).ToList();
            AllCategories.Insert(0, new Category { CategoryId = -1, Title = "--- Please select a category ---" });
            return AllCategories;
        }
    }
}
