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

        public async Task InvokeAsync(HttpContext httpContext,
            IPackageContext dbContext,
            ICacheableGetter cacheableGetter)
        {
            Endpoint? endpoint = httpContext.GetEndpoint();
            if(endpoint is not null) {
                if (endpoint.Metadata.Where(meta => meta is CacheableAttribute).Any()) {
                    int tryies = 10;
                    DateTime time = DateTime.Now;
                    TimeSpan second = TimeSpan.FromSeconds(1);

                    while (tryies != 0) {
                        while (time + second > DateTime.Now) { }

                        time = DateTime.Now;
                        tryies--;
                    }
                    httpContext.Response.StatusCode = 201;
                    return;
                }
            }
            await _next(httpContext);
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
