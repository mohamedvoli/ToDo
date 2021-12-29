using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.ViewModels
{
    public class EditTaskVm
    {
        public int TaskId { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Is it done already?")]
        public bool IsDone { get; set; }
        public List<Category> Categories { get; set; }
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }
        public Category SelectedCategory { get; set; }
    }
}
