using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace InstaCar.Web.Access
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }

            );
            /*config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/{controller}/login/{nn}/{pw}",
                defaults: new { nn = RouteParameter.Optional, pw = RouteParameter.Optional }

            );*/
        }
    }
}
