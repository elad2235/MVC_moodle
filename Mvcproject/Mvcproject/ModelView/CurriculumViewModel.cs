using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;

namespace Mvcproject.ModelView
{
    public class CurriculumViewModel
    {
        public Students student { get; set; }
        public Courses course { get; set; }
        public List<Courses> courses { get; set; }
        public Curriculum curriculum { get; set; }
        public List<Curriculum> curriculums { get; set; }

       
    }
}