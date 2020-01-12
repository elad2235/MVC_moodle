using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Moodle.Models
{
    public class Courses
    {
        [Key]
        public int course_id { get; set; }

        [Required]
        public string course_name { get; set; }
        [RegularExpression("^[1-9][0-9]{0,8}$", ErrorMessage = "Must have max of 9 digits")]
        public int teacher_id { get; set; }



        public string examA_class { get; set; }

        public string examB_class { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]

        public DateTime? moed_A { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? moed_B { get; set; }




    }

}

