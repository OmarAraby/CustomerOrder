using CustomerOrder.Core.Exceptions;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CustomerOrder.Api.Filters
{
    // to handle inputvalidation ex and will be global filter 
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid)
            {
                return;
            }

            var errors = actionContext.ModelState
                .Where(entry => entry.Value.Errors.Count > 0)
                .ToDictionary(
                    entry => StripParameterPrefix(entry.Key),
                    entry => entry.Value.Errors
                        .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                            ? "The value provided is not valid."
                            : error.ErrorMessage)
                        .ToArray());

            throw new InputValidationException(errors);
        }


        private static string StripParameterPrefix(string key)
        {
            var separator = key.IndexOf('.');
            return separator >= 0 && separator < key.Length - 1
                ? key.Substring(separator + 1)
                : key;
        }
    }
}