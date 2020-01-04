﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvcproject.Models
{
    public class Students
    {

        [Key]
        [Required]
        [RegularExpression("^[1-9][0-9]{0,8}$", ErrorMessage = "Must have max of 9 digits")]
        public int student_id{ get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string firstname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string lastname { get; set; }

        public string username { get; set; }


    }
}