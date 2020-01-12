using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models
{
    public class Grades
    {
        [Key]
        public int course_id { get; set; }
        [Key]
        public int student_id { get; set; }


        [RegularExpression("^(100|[1-9]?[0-9]|-1)?$", ErrorMessage = "Grade B must be between 0-100 or -1 if test not taken")]
        public int moedA { get; set; }
        [RegularExpression("^(100|[1-9]?[0-9]|-1)?$", ErrorMessage = "Grade B must be between 0-100 or -1 if test not taken")]
        public int moedB { get; set; }
    }
}