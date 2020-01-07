using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvcproject.Dal;

namespace Mvcproject.Models
{
    public class Students
    {

        [Key]
        [RegularExpression("^[1-9][0-9]{0,8}$", ErrorMessage = "Must have max of 9 digits")]
        public int student_id{ get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z]*$", ErrorMessage = "Student's name must contain only letters")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string firstname { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z]*$", ErrorMessage = "Student's name must contain only letters")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string lastname { get; set; }

        public string username { get; set; }

        public int get_MoedA_Grade_By_Courseid(int id)
        {
            GradesDal gdal = new GradesDal();
            int grade = (from x in gdal.Grades
                         where x.course_id == id && x.student_id == this.student_id
                         select x.moedA).FirstOrDefault();
            return grade;
        }

        public int get_MoedB_Grade_By_Courseid(int id)
        {
            GradesDal gdal = new GradesDal();
            int grade = (from x in gdal.Grades
                         where x.course_id == id && x.student_id == this.student_id
                         select x.moedB).FirstOrDefault();
            return grade;
        }
    }
}