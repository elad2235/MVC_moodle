using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvcproject.Models
{
    public class Courses
    {
        [Key]
        public int course_id { get; set; }

        [Required]
        public string course_name{ get; set; }

        public int teacher_id { get; set; }

        public DateTime moed_A { get; set; }

        public DateTime moed_B { get; set; }
    }
}