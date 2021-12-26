using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;

namespace TodoList.Models.Repos
{
    public class TaskRepo : ITodoRepo<TodoTask>
    {
        private ApplicationDbContext _db;
        public TaskRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Add(TodoTask entity)
        {
            _db.Tasks.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(TodoTask entity)
        {
            _db.Tasks.Remove(entity);
            _db.SaveChanges();
        }

        public void Edit(TodoTask entity)
        {
            _db.Tasks.Update(entity);
            _db.SaveChanges();
        }

        public TodoTask Find(int id)
        {
            var TaskNeeded = _db.Tasks.Include(x => x.ParentCategory).Include(x => x.User).SingleOrDefault(x => x.TodoTaskId == id);
            return TaskNeeded;
        }

        public List<TodoTask> List(string id)
        {
            var AllTasks = _db.Tasks.Where(x => (x.UserId == id) && (x.IsDone == false)).Include(x => x.ParentCategory).Include(x=> x.User).ToList();
            return AllTasks;
        }

        public List<TodoTask> ListFinishedTasks(string id)
        {
            var FinishedTasks = _db.Tasks.Where(x=>(x.UserId == id) && (x.IsDone == true) ).Include(x => x.ParentCategory).Include(x => x.User).ToList();
            return FinishedTasks;
        }

        public List<TodoTask> Search(string term, string id)
        {
            var AllTasks = _db.Tasks.Where(x => (x.Title.Contains(term) || x.Description.Contains(term)) && (x.UserId == id) && (x.IsDone == false)).Include(x => x.ParentCategory).Include(x => x.User).ToList();
            return AllTasks;
        }

        public List<TodoTask> SearchFinishedTasks(string term, string id)
        {
            var AllTasks = _db.Tasks.Where(x => (x.Title.Contains(term) || x.Description.Contains(term)) && (x.UserId == id) && (x.IsDone == true)).Include(x => x.ParentCategory).Include(x => x.User).ToList();
            return AllTasks;
        }
    }
}
