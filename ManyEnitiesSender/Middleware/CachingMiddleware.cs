using System.Collections.Concurrent;

using ManyEntitiesSender.Attributes;
using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.DAL.Interfaces;

using Microsoft.AspNetCore.Http.Connections;

namespace ManyEntitiesSender.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;
        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static ConcurrentDictionary<string, Mutex> monitors = new() 
        {

        };

        public async Task InvokeAsync(HttpContext httpContext,
            IPackageContext dbContext,
            ICacheableGetter cacheableGetter)
        {

            
            Endpoint? endpoint = httpContext.GetEndpoint();
            if(endpoint is not null) {
                if (endpoint.Metadata.Where(meta => meta is CacheableAttribute).Any()) {

                    monitors.GetOrAdd("body", new Mutex()).WaitOne();

                    await _next(httpContext);
                    httpContext.Response.StatusCode = 201;
                    monitors.GetOrAdd("body", new Mutex()).ReleaseMutex();
                }
                else {
                    await _next(httpContext);
                }
            }
        }
    }

    public static class CachingMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCaching(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CachingMiddleware>();
        }
    }
}
