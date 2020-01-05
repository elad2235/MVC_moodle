using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvcproject.Models
{
    public class Grades
    {
        [Key]
        public int course_id { get; set; }
        [Key]
        public int student_id { get; set; }

        [RegularExpression("^100|[1-9][0-9]|[0]$", ErrorMessage = "Grade A must be between 0-100")]
        public int moedA { get; set; }
        [RegularExpression("^100|[1-9][0-9]|[0]$", ErrorMessage = "Grade B must be between 0-100")]
        public int moedB { get; set; }
    }
}