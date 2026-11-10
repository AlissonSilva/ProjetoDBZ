using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDBZ.Middleware
{
    public class CorrelationIdMiddleware
    {
        
        private const string HeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId;

            if (context.Request.Headers.TryGetValue(HeaderName, out var requestId) && Guid.TryParse(requestId.FirstOrDefault(), out var parsedId))
            {
                correlationId = parsedId.ToString();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }

            // Disponibiliza o identificador durante toda a requisição.
            context.TraceIdentifier = correlationId;

            // Devolve o identificador na resposta HTTP.
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = correlationId;
                return Task.CompletedTask;
            });

            // Inclui o identificador nos logs gerados durante a requisição.
            using (_logger.BeginScope("CorrelationId: {CorrelationId}", correlationId))
            {
                await _next(context);
            }
        }
    }
}