using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Mvcproject.Models
{
    public class Studies
    {
        [Key]
        public int course_id { get; set; }
        
        [Key]
        public int student_id { get; set; }
     
    }
}