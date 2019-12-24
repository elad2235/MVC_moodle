using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;

namespace Mvcproject.ModelView
{
    public class TeachersViewModel
    {
        public Teachers teacher { get; set; }

        public List<Teachers> teachers { get; set; }
    }
}
