using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;
using System.Data.Entity;

namespace Mvcproject.Dal
{
    public class CoursesDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Courses>().ToTable("Courses");
        }
        public DbSet<Courses> Courses { get; set; }
    }
}



