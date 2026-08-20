using Serilog;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOrder.Api.Handlers
{
    public class RequestLoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Log.Information(
                "Request: {Method} {Url}",
                request.Method,
                request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                Log.Information(
                    "Success Response: {StatusCode}",
                    response.StatusCode);
            }
            else
            {
                Log.Error(
                    "Failed Response: {StatusCode}",
                    response.StatusCode);
            }

            return response;
        }
    }
}