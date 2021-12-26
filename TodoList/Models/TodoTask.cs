using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models
{
    public class TodoTask
    {
        public int TodoTaskId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [BindNever]
        [Display(Name ="Created At")]
        public DateTime TimeStamp { get; set; }
        [Display(Name = "Is it Done?")]
        public bool IsDone { get; set; }
        [Required]
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public Category ParentCategory { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
