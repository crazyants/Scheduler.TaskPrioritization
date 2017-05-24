namespace Scheduler.TaskPrioritization
{
    using System.Web.Http;
    using System.Web.Http.Cors;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.EnableCors(new EnableCorsAttribute("http://scheduler.albatros.bg", "*", "GET,POST", "Execution-Time"));
            config.EnableCors(new EnableCorsAttribute("http://localhost:4444", "*", "GET,POST", "Execution-Time"));

            config.Routes.MapHttpRoute(name: "DefaultApi",
                                       routeTemplate: "api/{controller}/{id}",
                                       defaults: new { id = RouteParameter.Optional });
        }
    }
}