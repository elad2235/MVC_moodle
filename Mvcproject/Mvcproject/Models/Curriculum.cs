using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Mvcproject.Models
{
    public class Curriculum
    {
        [Key]
        public int course_id { get; set; }
        
        public string day { get; set; }

        public string hour { get; set; }
        
        public string classroom { get; set; }

        public int duration { get; set; }
    }
}