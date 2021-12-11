using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Category> categories { get; set; }
        public ICollection<TodoTask> Tasks { get; set; }
    }
}
