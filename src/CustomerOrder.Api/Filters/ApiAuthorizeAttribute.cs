using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using CustomerOrder.Application.Common;

namespace CustomerOrder.Api.Filters
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal;

            var isAuthenticated = principal != null
                && principal.Identity != null
                && principal.Identity.IsAuthenticated;

            // Authenticated but wrong role -> 403. No identity at all -> 401.
            var statusCode = isAuthenticated ? HttpStatusCode.Forbidden : HttpStatusCode.Unauthorized;

            var message = isAuthenticated
                ? "You do not have permission to perform this action."
                : "Authentication is required.";

            actionContext.Response = actionContext.Request.CreateResponse(
                statusCode,
                ApiResponse.ErrorResponse(message));
        }
    }
}
