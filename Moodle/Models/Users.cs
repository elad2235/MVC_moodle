using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Moodle.Models
{
    public class Users
    {
        [Key]
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Username must begin with a letter and without special letters")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password" )]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string password { get; set; }

        public int permissions { get; set; }
    }
}