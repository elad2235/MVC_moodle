using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvcproject.Dal;

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

        public List<Students> get_Students_Of_Course()
        {
            StudentsDal sdal = new StudentsDal();
            StudiesDal stdal = new StudiesDal();
            List<int> students_id = (from x in stdal.Studies
                                     where x.course_id == this.course_id
                                     select x.student_id).ToList<int>();
            List<Students> students = new List<Students>();
            foreach (int s in students_id)
            {
                students.Add((from x in sdal.Students
                              where x.student_id == s
                              select x).FirstOrDefault());
            }
            return students;
        }
    }

}

