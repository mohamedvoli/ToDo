using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;

namespace TodoList.Models.Repos
{
    public class CategoryRepo : ITodoRepo<Category>
    {
        private ApplicationDbContext db;
        public CategoryRepo(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(Category entity)
        {
            db.Categories.Add(entity);
            db.SaveChanges();
        }

        public void Delete(Category entity)
        {
            db.Categories.Remove(entity);
            db.SaveChanges();
        }

        public void Edit(Category entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }

        public Category Find(int id)
        {
            var category = db.Categories.SingleOrDefault(a => a.CategoryId == id);
            return category;
        }

        public List<Category> List(string id)
        {
            var categories = db.Categories.Where(a =>a.UserId == id ).ToList();
            return categories;
        }
        public List<Category> Search(string term,string id)
        {
            var Categories = db.Categories.Where(x => (x.Title.Contains(term)) && (x.UserId == id)).ToList();
            return Categories;
        }
    }
}
