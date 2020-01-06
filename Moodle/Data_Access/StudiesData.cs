using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Moodle.Models;

namespace Moodle.Data_Access
{
    public class StudiesData : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Studies>().ToTable("Studies").HasKey(t => new { t.course_id, t.student_id });
        }
        public DbSet<Studies> Studies { get; set; }

    }
}
