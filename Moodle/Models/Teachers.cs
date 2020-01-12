using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Moodle.Data_Access;

namespace Moodle.Models
{
    public class Teachers
    {

        [Key]
        public int teacher_id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string firstname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string lastname { get; set; }

        public string username { get; set; }


        public List<Curriculum> get_Courses_Schedule()
        {
            CoursesData cdal = new CoursesData();
            CurriculumData curdal = new CurriculumData();

            List<int> courses = (from x in cdal.Courses
                                 where x.teacher_id == this.teacher_id
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

        public List<Courses> get_Courses_List()
        {
            CoursesData cdal = new CoursesData();

            return ((from x in cdal.Courses
                     where x.teacher_id == this.teacher_id
                     select x).ToList<Courses>());

        }
    }
}