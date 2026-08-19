using CustomerOrder.Application.Common;
using CustomerOrder.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace CustomerOrder.Api.ErrorHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;
            var statusCode = ResolveStatusCode(exception);

            // A DomainException message is a deliberate contract with the caller.
            // Anything else could leak internals, so it never reaches the client.
            var message = exception is DomainException
                ? exception.Message
                : "An unexpected error occurred.";

            var response = context.Request.CreateResponse(
                statusCode,
                ApiResponse.ErrorResponse(message, ResolveErrors(exception)));

            context.Result = new ResponseMessageResult(response);
        }

        private static HttpStatusCode ResolveStatusCode(Exception exception)
        {
            switch (exception)
            {
                case InputValidationException _:
                    return HttpStatusCode.BadRequest;
                case NotFoundException _:
                    return HttpStatusCode.NotFound;
                case ConflictException _:
                    return HttpStatusCode.Conflict;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        private static List<string> ResolveErrors(Exception exception)
        {
            var validationException = exception as InputValidationException;

            if (validationException == null)
            {
                return new List<string>();
            }

            return validationException.Errors
                .SelectMany(entry => entry.Value.Select(message => entry.Key + ": " + message))
                .ToList();
        }
    }

}