using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproject.Models;

namespace Mvcproject.ModelBinders
{
    public class TeachersBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string teachersfirstname = objContext.Request.Form["firstname"];
            string teacherslastname = objContext.Request.Form["lastname"];


            Teachers obj = new Teachers()
            {
                firstname = teachersfirstname,
                lastname = teacherslastname


            };

            return obj;
        }
    }
}
    