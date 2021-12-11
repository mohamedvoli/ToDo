using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TodoList.Models;
using TodoList.Models.ViewModels;

namespace TodoList.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Mapping the relation for the Category model 1:M with Application User.
            modelBuilder.Entity<Category>()
     .HasOne<ApplicationUser>(p => p.ApplicationUser)
     .WithMany(s => s.categories)
     .HasForeignKey(p => p.UserId);
            //Mapping the relation for the TodoTasks model 1:M with Application User.
            modelBuilder.Entity<TodoTask>()
     .HasOne<ApplicationUser>(p => p.User)
     .WithMany(s => s.Tasks)
     .HasForeignKey(p => p.UserId);
            //Mapping the relation for the TodoTasks model 1:M with Application User.
            modelBuilder.Entity<TodoTask>()
     .HasOne<Category>(p => p.ParentCategory)
     .WithMany(s => s.ChildTasks)
     .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<TaskCategoryVm>()
                .HasNoKey();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<TodoTask> Tasks { get; set; }
        public DbSet<TodoList.Models.ViewModels.TaskCategoryVm> TaskCategoryVm { get; set; }
    }
}
