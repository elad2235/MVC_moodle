using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvcproject.Models;

namespace Mvcproject.ModelView
{
    public class UsersViewModel
    {
        public Users user { get; set; }

        public List<Users> users{ get; set; }

        public Students student { get; set; }

        public List<Students> students { get; set; }


    }
}