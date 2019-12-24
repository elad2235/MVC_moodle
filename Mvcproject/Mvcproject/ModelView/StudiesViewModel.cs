using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;

namespace Mvcproject.ModelView
{
    public class StudiesViewModel
    {

        public Courses course { get; set; }

        public Students student { get; set; }

        public Studies study { get; set; }

        public List<Studies> studies { get; set; }
      
    }
}


