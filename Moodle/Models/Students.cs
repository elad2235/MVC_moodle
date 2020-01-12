using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Moodle.Data_Access;

namespace Moodle.Models
{
    public class Students
    {
        [Key]
        public int student_id { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z]*$", ErrorMessage = "Student's name must contain only letters")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string firstname { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z][a-zA-Z]*$", ErrorMessage = "Student's name must contain only letters")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string lastname { get; set; }

        public string username { get; set; }


        public List<Curriculum> get_Courses_Schedule()
        {
            StudiesData cdal = new StudiesData();
            CurriculumData curdal = new CurriculumData();

            List<int> courses = (from x in cdal.Studies
                                 where x.student_id == this.student_id
                                 select x.course_id).ToList<int>();

            List<Curriculum> curs = new List<Curriculum>();
            foreach (int c in courses)
            {
                List<Curriculum> tmp = (from x in curdal.Curriculum
                                        where x.course_id == c
                                        select x).ToList<Curriculum>();
                if (tmp != null)
                {
                    foreach (Curriculum c1 in tmp)
                    {
                        curs.Add(c1);
                    }
                }
            }
            return curs;
        }
    }
}