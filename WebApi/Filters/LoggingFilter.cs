using System.Diagnostics;
using DataAccess.Concrete.SQLServer;
using Entities.Concrete.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Core.Utilities.Static;
using Core.Helpers;

namespace WebApi.Filters
{
    public class LoggingFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();
            Int64 executionTimeMs = 0;
            var response = await next();

            stopwatch.Stop();
            executionTimeMs = stopwatch.ElapsedMilliseconds;
            var path = $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}";
            var ipAddress = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                            ?? context.HttpContext.Connection.RemoteIpAddress?.ToString();

            string authHeader = context.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault();

            string agent = authHeader is null ? null : authHeader.Split(' ')[1].DecodeJwt();

            if (response.Result is ObjectResult objectResult && objectResult.Value is Core.Utilities.Results.Abstract.IResult result)
            {
                var statusCode = result.StatusCode;

                LogEntity logEntity = new LogEntity
                {
                    IsSucces = result.Success,
                    StatusCode = result.StatusCode,
                    Message = result.Message is null ? "Fine" : result.Message,
                    Path = path,
                    IpAddres = ipAddress,
                    ExecutingTime = executionTimeMs,
                    ExceptionMessage = result.Exception == null ? "Fine" : result.Exception.InnerException is null ? result.Exception.Message : result.Exception.InnerException.Message,
                    ErrorMessage = result.ErrorMessage is null ? "Fine" : result.ErrorMessage,
                    StackTrace = result.Exception is null ? "Fine" : result.Exception.StackTrace,
                    Agent = agent
                };

                if (!logEntity.Path.StartsWith("/nam/Log/Logs"))
                { 
                    using (var dBcontext = new AppDbContext())
                    {
                        Log log = logEntity.Map<Log, LogEntity>();

                        log.Date = DateTime.UtcNow.AddHours(4);

                        await dBcontext.Logs.AddAsync(log);

                        await dBcontext.SaveChangesAsync();
                    }
                    result.Exception = null;
                }
            }

        }
    }
}