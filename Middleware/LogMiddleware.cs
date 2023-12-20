using System.Diagnostics;
using System;
// using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
using System.IO;


namespace Middleware{
    
public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string logFilePath;

        public LogMiddleware(RequestDelegate next, string logFilePath)
        {
            this.next = next;
            this.logFilePath = logFilePath;
        }

        public async Task Invoke(HttpContext c)
        {
            var act = $"{c.Request.Path}.{c.Request.Method}";
            var sw = new Stopwatch();
            sw.Start();

            string logMessage=($"{act} started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            WriteLogToFile(logMessage);
            await next.Invoke(c);
            logMessage=($"{act} ended at {sw.ElapsedMilliseconds} ms. UserId: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
            WriteLogToFile(logMessage);
        }
        public void WriteLogToFile(string logMessage){
            using (StreamWriter sw = File.AppendText(logFilePath)){
                sw.WriteLine(logMessage);
            }
        }
    }

    public static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder,string logFilePath)
        {
            return builder.UseMiddleware<LogMiddleware>(logFilePath);
        }
    }
}

    

