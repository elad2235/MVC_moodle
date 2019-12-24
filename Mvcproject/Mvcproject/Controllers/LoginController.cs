using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproject.Models;
using Mvcproject.ModelView;
using Mvcproject.Dal;
using Mvcproject.ModelBinders;





namespace Mvcproject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login


        
        public ActionResult HomePageStudent()
        {
          
            return View();
        }

       

     
        public ActionResult LoginPage()
        {
            UsersDal dal = new UsersDal();
            List<Users> objUsers = dal.Users.ToList<Users>();
            UsersViewModel uvm = new UsersViewModel();
            uvm.user = new Users();
            uvm.users = objUsers;
            return View(uvm);
        }

        [HttpPost]
        public ActionResult Login()
        {

            UsersDal dal = new UsersDal();

            string searchUsername = Request.Form["user.username"].ToString();
            string searchPassword = Request.Form["user.password"].ToString();
            List<Users> objUsers =
                (from x in dal.Users
                 where x.username.Equals(searchUsername) && x.password.Equals(searchPassword)
                 select x).ToList<Users>();
            UsersViewModel uvm = new UsersViewModel();
            uvm.users = objUsers;

            Session["username"] =uvm.users[0].username;
            Session["permission"] = uvm.users[0].permissions;
            if (objUsers.Count!=0)
            {
                if (objUsers[0].permissions == 1) { return RedirectToAction("HomePageAdmin", "Admin"); }
                if (objUsers[0].permissions== 2) { return RedirectToAction("HomePageTeacher", "Teacher"); }
                return RedirectToAction("HomePageStudent", "Student");
            }
            
            return View("LoginPage");

        }
    }
}