using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Moodle.Models;
namespace Moodle.Data_Access
{
    public class CoursesData : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Courses>().ToTable("Courses");
        }
        public DbSet<Courses> Courses { get; set; }
    }
}