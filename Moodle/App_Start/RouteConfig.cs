using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moodle
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
            name: "FacultyManager",
            url: "FacultyManager",
            defaults: new { controller = "FacultyManager", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Teacher",
            url: "Teacher",
            defaults: new { controller = "Teacher", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Student",
            url: "Student",
            defaults: new { controller = "Student", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "remove_lesson", 
            url: "editCourse",
            defaults: new { controller = "FacultyManager", action = "remove_lesson", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
        );
        }
    }
}
