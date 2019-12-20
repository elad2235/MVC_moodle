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
        public ActionResult Submit()
        {
          
            UsersViewModel uvm = new UsersViewModel();
            Users objUsers = new Users();
            objUsers.username = Request.Form["user.username"].ToString();
            objUsers.password = Request.Form["user.password"].ToString();
            UsersDal dal = new UsersDal();

            if (ModelState.IsValid)
            { //checks if the post from the form is valid


                dal.Users.Add(objUsers); // in memory adding
                dal.SaveChanges();
                uvm.user = new Users();


            }
            else
            {
                // return View("Enter", cust); //sending cust because enter refreshes when there are vallidations errors
                uvm.user = objUsers;
              
            }
            uvm.users = dal.Users.ToList<Users>();
            return View("LoginPage", uvm);
        }
    }
}