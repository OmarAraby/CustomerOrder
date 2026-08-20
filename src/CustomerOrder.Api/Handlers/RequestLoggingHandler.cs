using Serilog;
using System;
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
            var correlationId = Guid.NewGuid().ToString("N").Substring(0, 8);

            // Stashed so the GlobalExceptionHandler can surface it as a traceId in M8.
            request.Properties["CorrelationId"] = correlationId;

            Log.Information(
                "Request: {CorrelationId} {Method} {Url}",
                correlationId,
                request.Method,
                request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                Log.Information(
                    "Success Response: {CorrelationId} {StatusCode}",
                    correlationId,
                    response.StatusCode);
            }
            else
            {
                Log.Error(
                    "Failed Response: {CorrelationId} {StatusCode}",
                    correlationId,
                    response.StatusCode);
            }

            return response;
        }
    }
}