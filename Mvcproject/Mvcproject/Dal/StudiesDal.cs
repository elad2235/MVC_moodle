using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;
using System.Data.Entity;

namespace Mvcproject.Dal
{
    public class StudiesDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Studies>().ToTable("Studies").HasKey(t => new { t.course_id, t.student_id }); 
        }
        public DbSet<Studies> Studies{ get; set; }
     
    }
}



