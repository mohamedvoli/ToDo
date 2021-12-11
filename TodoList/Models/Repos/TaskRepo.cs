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
            var TaskNeeded = _db.Tasks.SingleOrDefault(x=> x.TodoTaskId == id);
            return TaskNeeded;
        }

        public List<TodoTask> List(string id)
        {
            var AllTasks = _db.Tasks.Where(x => x.UserId == id).ToList();
            return AllTasks;
        }

        public List<TodoTask> Search(string term, string id)
        {
            var AllTasks = _db.Tasks.Where(x => (x.Title.Contains(term) || x.Description.Contains(term)) && (x.UserId == id)).ToList();
            return AllTasks;
        }
    }
}
