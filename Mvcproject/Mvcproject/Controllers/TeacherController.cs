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
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult HomePageTeacher()
        {

            return View();
        }
    }
}