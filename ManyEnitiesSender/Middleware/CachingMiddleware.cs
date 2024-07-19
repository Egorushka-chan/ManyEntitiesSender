using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

using ManyEntitiesSender.Attributes;
using ManyEntitiesSender.BLL.Services.Abstractions;
using ManyEntitiesSender.DAL.Entities;
using ManyEntitiesSender.DAL.Interfaces;
using ManyEntitiesSender.Models;
using ManyEntitiesSender.Models.Responses;

using Microsoft.AspNetCore.Http.Connections;

using StackExchange.Redis;

namespace ManyEntitiesSender.Middleware
{
    public class CachingMiddleware
    {
        private readonly RequestDelegate _next;
        public CachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Мьютексы, мониторы, lock - они все требуют работы в синхронном контексте - т.е между wait и release не должно быть await
        //private static ConcurrentDictionary<string, Mutex> monitors = new();

        // А SemaphoreSlim вообще нормально
        private static ConcurrentDictionary<string, SemaphoreSlim> semaphores = new();

        /// <summary>
        /// Если возвращает 200 - это контроллер создал, если 201 - то создал Redis
        /// </summary>
        public async Task InvokeAsync(HttpContext httpContext, IRedisProvider redis)
        {
            Endpoint? endpoint = httpContext.GetEndpoint();
            if(endpoint is not null) {
                if (endpoint.Metadata.Where(meta => meta is CacheableAttribute).Any())
                {
                    string? table = (string)httpContext.Items["table"];
                    if(table == null)
                        throw new NotFoundMiddlewareException();
                    string? filter = (string?)httpContext.Items["filter"];
                    
                    bool redisReturnedValue = await CheckRedis(httpContext, redis, table, filter);
                    if (redisReturnedValue)
                    {
                        return;
                    }
                    try
                    {
                        //monitors.GetOrAdd(table, new Mutex()).WaitOne(); // здесь происходит магия 2check
                        semaphores.GetOrAdd(table, new SemaphoreSlim(1,1)).Wait();
                        bool redisReturnedValueSecond = await CheckRedis(httpContext, redis, table, filter); 
                        if (redisReturnedValueSecond)
                        {
                            // httpContext.Response.StatusCode = 201;
                        }
                        else
                        {
                            string responseBody;
                            // супер трюк
                            Stream originalBody = httpContext.Response.Body;
                            using (var memStream = new MemoryStream())
                            {
                                httpContext.Response.Body = memStream;

                                await _next(httpContext);

                                memStream.Position = 0;
                                responseBody = new StreamReader(memStream).ReadToEnd();

                                memStream.Position = 0;
                                await memStream.CopyToAsync(originalBody);
                                httpContext.Response.Body = originalBody;
                            }
                            await CacheResponseBody(redis, table, responseBody);
                        }
                    }
                    finally
                    {
                        //monitors.GetOrAdd(table, new Mutex()).ReleaseMutex();
                        semaphores.GetOrAdd(table, new SemaphoreSlim(0,1)).Release();
                    }
                }
                else {
                    // если endpoint без нужного атрибута, просто его пропускаем
                    await _next(httpContext);
                }
            }
        }

        private static async Task CacheResponseBody(IRedisProvider redis, string? table, string responseBody)
        {
            if (table == "body")
            {
                Body[] bodies = JsonSerializer.Deserialize<Body[]?>(responseBody) ?? throw new ArgumentNullException(nameof(responseBody));
                await redis.AppendListAsync(bodies);
            }
            else if (table == "hand")
            {
                Hand[] hands = JsonSerializer.Deserialize<Hand[]?>(responseBody) ?? throw new ArgumentNullException(nameof(responseBody));
                await redis.AppendListAsync(hands);
            }
            else if (table == "leg")
            {
                Leg[] legs = JsonSerializer.Deserialize<Leg[]?>(responseBody) ?? throw new ArgumentNullException(nameof(responseBody));
                await redis.AppendListAsync(legs);
            }
        }

        /// <summary>
        /// Кушает данные из стрима иии.. соединяет их в один пакет. Иначе логика потребуется очень сложная
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> CheckRedis(HttpContext httpContext, IRedisProvider redis, string? requestedTable, string? filterValue)
        {
            bool redisReturnedValue = false;
            if (requestedTable.ToLower() == "body")
            {
                List<Body> values = new List<Body>();
                await foreach (var package in redis.TryGetAsync<Body>(filterValue))
                {
                    if (package is null)
                        break;
                    if (package.Count == 0)
                        break;

                    values.AddRange(package);

                    redisReturnedValue = true;
                }
                if (values.Count > 0) {
                    httpContext.Response.StatusCode = 201;
                    await httpContext.Response.WriteAsJsonAsync(values);
                }
            }
            if (requestedTable.ToLower() == "hand")
            {
                List<Hand> values = new List<Hand>();
                await foreach (var package in redis.TryGetAsync<Hand>(filterValue))
                {
                    if (package is null)
                        break;
                    if (package.Count == 0)
                        break;

                    values.AddRange(package);

                    redisReturnedValue = true;
                }
                if (values.Count > 0) {
                    httpContext.Response.StatusCode = 201;
                    await httpContext.Response.WriteAsJsonAsync(values);
                }
                    
            }
            if (requestedTable.ToLower() == "leg")
            {
                List<Leg> values = new List<Leg>();
                await foreach (var package in redis.TryGetAsync<Leg>(filterValue))
                {
                    if (package is null)
                        break;
                    if (package.Count == 0)
                        break;

                    values.AddRange(package);

                    redisReturnedValue = true;
                }
                if (values.Count > 0) {
                    httpContext.Response.StatusCode = 201;
                    await httpContext.Response.WriteAsJsonAsync(values);
                }
            }

            return redisReturnedValue;
        }
    }

    public static class CachingMiddlewareExtensions
    {
        /// <summary>
        /// Должно идти после UseMyCachingValidation
        /// </summary>
        public static IApplicationBuilder UseMyCaching(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CachingMiddleware>();
        }
    }
}
