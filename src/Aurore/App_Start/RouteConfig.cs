using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Aurore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "HomeController",
                url: "{id}",
                defaults: new { controller = "Home", action = "Index", id = string.Empty }
            );

            routes.MapRoute(
                name: "StatController",
                url: "stat/{id}",
                defaults: new { controller = "Status", action = "Index", id = string.Empty }
            );

            routes.MapRoute(
                name: "PWController",
                url: "pw/{id}",
                defaults: new { controller = "Password", action = "Index", id = string.Empty }
            );
        }
    }
}