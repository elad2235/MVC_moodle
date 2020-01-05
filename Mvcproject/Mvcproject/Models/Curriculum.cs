using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mvcproject.Dal;
using Mvcproject.Models;

namespace Mvcproject.Models
{
    public class Curriculum
    {
        [Key]
        public int id { get; set; }
        public int course_id { get; set; }

        public string day { get; set; }

        public string hour { get; set; }
        
        public string classroom { get; set; }

        public int duration { get; set; }

        public  string getCname()
        {
            CoursesDal cdal = new CoursesDal();
            string name = (from x in cdal.Courses
                           where x.course_id == this.course_id
                           select x.course_name).FirstOrDefault();
            return name;
        }
    }

   
}