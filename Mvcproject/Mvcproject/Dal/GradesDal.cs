using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;
using System.Data.Entity;

namespace Mvcproject.Dal
{
    public class GradesDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Grades>().ToTable("Grades").HasKey(t => new { t.course_id, t.student_id });
        }
        public DbSet<Grades> Grades { get; set; }

    }
}



