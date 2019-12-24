using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvcproject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
           name: "HomePageStudent",
           url: "HomePageStudent",
           defaults: new { controller = "Login", action = "HomePageStudent", id = UrlParameter.Optional }
       );

            routes.MapRoute(
         name: "HomePageAdmin",
         url: "HomePageAdmin",
         defaults: new { controller = "Login", action = "HomePageAdmin", id = UrlParameter.Optional }
     );


            routes.MapRoute(
         name: "HomePageTeacher",
         url: "HomePageTeacher",
         defaults: new { controller = "Login", action = "HomePageTeacher", id = UrlParameter.Optional }
     );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "LoginPage", id = UrlParameter.Optional }
            );

         
        }
    }
}
