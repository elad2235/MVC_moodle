using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Moodle.Models
{
    public class Curriculum
    {
        [Key]
        public int id { get; set; }
        public int course_id { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]

        public DateTime? time { get; set; }

        public string classroom { get; set; }

        public int duration { get; set; }
    }
}