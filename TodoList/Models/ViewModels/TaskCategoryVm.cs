using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.ViewModels
{
    public class TaskCategoryVm
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsDone { get; set; }
        public List<Category> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
