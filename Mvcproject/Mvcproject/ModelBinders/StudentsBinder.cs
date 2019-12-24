using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproject.Models;

namespace Mvcproject.ModelBinders
{
    public class StudentsBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string studentfirstname = objContext.Request.Form["firstname"];
            string studentlastname = objContext.Request.Form["lastname"];


            Students obj = new Students()
            {
                firstname = studentfirstname,
                lastname = studentlastname


            };

            return obj;
        }
    }
}
