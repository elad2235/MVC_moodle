using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;

namespace Mvcproject.ModelView
{
    public class StudentsViewModel
    {
        public Students student { get; set; }

        public List<Students> students { get; set; }
    }
}

