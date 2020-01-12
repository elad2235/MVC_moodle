using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moodle.Models;
using System.Security.Cryptography;
using Moodle.Data_Access;
using BCrypt;

namespace Moodle.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult Login()
        {
            return View("Login");
        }


        public ActionResult validate_user(Users user)
        {

            UsersData users = new UsersData();
            string controller = "Login";

            if (ModelState.IsValid)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);

                Users isUser = users.Users.Where(x => x.username == user.username).FirstOrDefault();

                if (isUser != null && BCrypt.Net.BCrypt.Verify(user.password, hashedPassword))
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



            return RedirectToRoute(controller, controller);
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


                case 3:
                    return "Student";

            }

            return  "Login";
        }


    }


}



