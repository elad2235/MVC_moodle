using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;
using System.Data.Entity;

namespace Mvcproject.Dal
{
    public class CurrDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Curriculum>().ToTable("Curriculum");
        }
        public DbSet<Curriculum> Curriculum { get; set; }
    }
}