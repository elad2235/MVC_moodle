using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproject.Models;

namespace Mvcproject.ModelBinders
{
    public class UsersBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            string userusername = objContext.Request.Form["username"];
            string userpassword = objContext.Request.Form["password"];
            

            Users obj = new Users()
            {
                username = userusername,
                password = userpassword
              

            };

            return obj;
        }
    }
}