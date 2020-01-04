using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvcproject.Models
{
    public class Teachers
    {
        [Key]
        public int  teacher_id{ get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string firstname { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string lastname { get; set; }


    }
}