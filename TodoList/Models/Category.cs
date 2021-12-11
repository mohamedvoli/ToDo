using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [Display(Name ="Category Title")]
        public string Title { get; set; }
        public string UserId  { get; set; }
        public ApplicationUser ApplicationUser  { get; set; }
        public ICollection<TodoTask> ChildTasks { get; set; }
    }
}
