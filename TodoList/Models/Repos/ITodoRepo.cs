using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.Repos
{
    public interface ITodoRepo<Entity>
    {
        public Entity Find(int id);
        public void Add(Entity entity);
        public void Edit(Entity entity);
        public void Delete(Entity entity);
        public List<Entity> List(string id);
        public List<Entity> ListFinishedTasks(string id);
        public List<Entity> Search(string term,string id);
        public List<Entity> SearchFinishedTasks(string term,string id);

    }
}
