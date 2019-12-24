using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;
using System.Data.Entity;

namespace Mvcproject.Dal
{
    public class Faculty_Administrators_Dal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Faculty_Administrators>().ToTable("Faculty_Administrators");
        }
        public DbSet<Faculty_Administrators> Faculty_Administrators { get; set; }
    }
}
