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
        [RegularExpression("^[1-9][0-9]{0,8}$", ErrorMessage = "Must have max of 9 digits")]
        public int teacher_id { get; set; }

        [RegularExpression("^([0-9]/|[12][0-9]/|30/)(1[0-2]/|[0-9]/)202[0-9]$", ErrorMessage = "Date should be in the form of [DD/MM/YYYY]")]
        public string moed_A_date { get; set; }
        [RegularExpression("^[0-9]{2}:[0-9]{2}$", ErrorMessage = "Time should be in the form of [HH:MM]")]
        public string moed_A_hour { get; set; }
        [RegularExpression("^([0-9]/|[12][0-9]/|30/)(1[0-2]/|[0-9]/)202[0-9]$", ErrorMessage = "Date should be in the form of [DD/MM/YYYY]")]
        public string moed_B_date { get; set; }
        [RegularExpression("^[0-9]{2}:[0-9]{2}$", ErrorMessage = "Time should be in the form of [HH:MM]")]
        public string moed_B_hour { get; set; }

       
    }

}

