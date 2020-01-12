using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Moodle.Models;

namespace Moodle.Data_Access
{
    public class StudentsData :DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Students>().ToTable("Students");
        }
        public DbSet<Students> Students { get; set; }
    }
}