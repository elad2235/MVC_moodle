using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproject.Models;

namespace Mvcproject.ModelBinders
{
    public class FacultyBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string facultyfirstname = objContext.Request.Form["firstname"];
            string facultylastname = objContext.Request.Form["lastname"];


            Faculty_Administrators obj = new Faculty_Administrators()
            {
                firstname = facultyfirstname,
                lastname = facultylastname


            };

            return obj;
        }
    }
}
    