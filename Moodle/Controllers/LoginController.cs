using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moodle.Models;
using System.Security.Cryptography;
using Moodle.Data_Access;

namespace Moodle.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }


        public ActionResult validate_user(Users user)
        {

            UsersData users = new UsersData();
            string controller = "Login";

            if (ModelState.IsValid)
            {

                Users isUser = users.Users.Where(x => x.username == user.username && user.password == x.password).FirstOrDefault();

                if (isUser != null)
                {
                    Session["username"] = isUser.username;
                    Session["permission"] = isUser.permissions;
                    controller = userRouting(isUser.permissions);
                }
                else
                {
                    ModelState.AddModelError("", "Username or password are incorrect");
                    return View("Login");
                }
            }



            return RedirectToRoute("Index", controller);
        }



        //Route User to correct controller 
        public string userRouting(int perm)
        {
            switch (perm)
            {
                case 0:
                    return "Admin";


                case 1:
                    return "FacultyManager";


                case 2:
                    return "Teacher";


                case 4:
                    return"Student";

            }

            return  "Login";
        }


    }


}



