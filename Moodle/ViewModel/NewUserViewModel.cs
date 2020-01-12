using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moodle.Models;

namespace Moodle.ViewModel
{
    public class NewUserViewModel
    {
        public Users user { get; set; }

        public Students student { get; set; }
    }
}