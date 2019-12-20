using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvcproject.Models
{
    public class Users
    {
        [Key]
        [Required]
        [StringLength(50,MinimumLength =2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string username { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string password { get; set; }

  
    }
}