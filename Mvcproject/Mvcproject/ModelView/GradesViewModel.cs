using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;

namespace Mvcproject.ModelView
{
    public class GradesViewModel
    {
        public Grades grade { get; set; }
        public List<Grades> grades { get; set; }
    }
}