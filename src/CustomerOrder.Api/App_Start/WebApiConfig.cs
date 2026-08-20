using CustomerOrder.Api.ErrorHandling;
using CustomerOrder.Api.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace CustomerOrder.Api
{
   
    // our dependency resolver is set up in AutofacConfig.Register, which is called first.
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // DI first - everything after this can rely on the resolver being in place.
            AutofacConfig.Register(config);

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());  // don't for get replace instead of add 

            // idenity auth
            config.Filters.Add(new ApiAuthorizeAttribute());

            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new ValidateFluentAttribute());

            // Enables [Route] / [RoutePrefix]. Needed for the nested
            // /api/orders/{id}/customers/{customerId} endpoints 
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // JSON only 
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var json = config.Formatters.JsonFormatter.SerializerSettings;

            
            json.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.NullValueHandling = NullValueHandling.Ignore;
            // OrderStatus serialises as "Pending" instead of 1.
            json.Converters.Add(new StringEnumConverter());
            json.Formatting = Formatting.Indented;
        }
    }
}
