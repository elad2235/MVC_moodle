using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moodle.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            if (!isStudent())
                return RedirectToRoute("Login");
            return View();
        }


        private bool isStudent()
        {
            var perm = 0;
            try
            {
                perm = Int32.Parse(Session["permission"].ToString());
            }
            catch (Exception e)
            {
                return false;
            }

            if ( perm != 3)
                return false;

            return true;

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToRoute("Login");
        }

    }


}