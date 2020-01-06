using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models
{
    public class Studies
    {
            [Key]
            public int course_id { get; set; }

            [Key]
            public int student_id { get; set; }
    }
}