using ManyEntitiesSender.Attributes;
using ManyEntitiesSender.DAL.Interfaces;
using ManyEntitiesSender.Models.Responses;
using System.Threading;

namespace ManyEntitiesSender.Middleware
{
    public class CachingValidationMiddleware
    {
        private readonly RequestDelegate _next;
        public CachingValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IRedisProvider redis)
        {
            Endpoint? endpoint = httpContext.GetEndpoint();
            if(endpoint is not null) {
                if (endpoint.Metadata.Where(meta => meta is CacheableAttribute).Any())
                {
                    string? requestedTable = httpContext.Request.Query["table"];
                    if (requestedTable is null)
                    {
                        httpContext.Response.StatusCode = 400;
                        await httpContext.Response.WriteAsJsonAsync(new ErrorResponse() { Body = "No table property specified = no request" });
                        return;
                    }

                    requestedTable = requestedTable.ToLower();
                    string[] allowedTables = {"body", "hand", "leg"};

                    if (!allowedTables.Contains(requestedTable))
                    {
                        httpContext.Response.StatusCode = 400;
                        await httpContext.Response.WriteAsJsonAsync(new ErrorResponse() { Body = $"Valid tables: {string.Join(", ", allowedTables)}." +
                            $" Your table {requestedTable} is not"});
                        return;
                    }

                    string? filterValue = httpContext.Request.Query["filter"];

                    string? skipValue = httpContext.Request.Query["skip"];
                    if (skipValue != null)
                    {
                        bool passed = int.TryParse(skipValue, out int skip);
                        if (!passed)
                        {
                            httpContext.Response.StatusCode = 400;
                            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse() { Body = $"Skip property must be integer. You sent: {skipValue}" });
                            return;
                        }
                        if(skip <= 0)
                        {
                            httpContext.Response.StatusCode = 400;
                            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse() { Body = $"Skip property must be > 0. You sent: {skip}" });
                            return;
                        }
                        httpContext.Items["skip"] = skip;
                    }
                    

                    string? takeValue = httpContext.Request.Query["skip"];
                    if(takeValue != null)
                    {
                        bool passed = int.TryParse(takeValue, out int take);
                        if (!passed)
                        {
                            httpContext.Response.StatusCode = 400;
                            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse() { Body = $"Take property must be integer. You sent: {takeValue}" });
                            return;
                        }
                        if(take <= 0)
                        {
                            httpContext.Response.StatusCode = 400;
                            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse() { Body = $"Take property must be > 0. You sent: {take}" });
                            return;
                        }
                        httpContext.Items["take"] = take;
                    }
                    
                    httpContext.Items["table"] = requestedTable;
                    httpContext.Items["filter"] = filterValue;
                    

                    await _next(httpContext);
                }
                else {
                    // если endpoint без нужного атрибута, просто его пропускаем
                    await _next(httpContext);
                }
            }
        }
    }

    public static class CachingValidationMiddlewareExtensions
    {
        /// <summary>
        /// Проверяет и в Items добавляет! Должно идти перед UseMyCaching
        /// </summary>
        public static IApplicationBuilder UseMyCachingValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CachingValidationMiddleware>();
        }
    }
}
