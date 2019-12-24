using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvcproject.Models
{
    public class Curriculum
    {
        public int course_id { get; set; }
        public DateTime time { get; set; }
        public string classroom { get; set; }
    }
}