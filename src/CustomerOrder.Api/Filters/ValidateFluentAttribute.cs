using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CustomerOrder.Core.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace CustomerOrder.Api.Filters
{
    /// <summary>
    /// Runs any registered FluentValidation validator for the action's arguments and
    /// funnels failures into the same InputValidationException the Data Annotations
    /// filter uses - so every validation source produces one response shape.
    /// </summary>
    public class ValidateFluentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var scope = actionContext.Request.GetDependencyScope();
            var failures = new List<ValidationFailure>();

            foreach (var argument in actionContext.ActionArguments.Values)
            {
                if (argument == null)
                {
                    continue;
                }

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = scope.GetService(validatorType) as IValidator;

      
                if (validator == null)
                {
                    continue;
                }

                var result = validator.Validate(new ValidationContext<object>(argument));

                if (!result.IsValid)
                {
                    failures.AddRange(result.Errors);
                }
            }

            if (failures.Count == 0)
            {
                return;
            }

            var errors = failures
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(failure => failure.ErrorMessage).ToArray());

            throw new InputValidationException(errors);
        }
    }
}
