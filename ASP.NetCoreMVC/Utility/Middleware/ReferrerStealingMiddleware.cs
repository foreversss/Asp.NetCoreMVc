using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreMVC.Utility.Middleware
{
    public class ReferrerStealingMiddleware
    {
        private readonly RequestDelegate _next;

        public ReferrerStealingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            //请求路径
            var url = context.Request.Path.Value;

            if (!url.Contains(".jpg"))
            {
                await _next.Invoke(context);
            }

            string urlReferrer = context.Request.Headers["Referer"];
            //如果直接访问 则不给访问
            if (string.IsNullOrWhiteSpace(urlReferrer))
            {
                await SetForbiddenImage(context);

            }
            //如果不是本机访问 则不给访问
            else if (!urlReferrer.Contains("localhost"))
            {
                await SetForbiddenImage(context);
            }
            else
            {
                await _next.Invoke(context);
            }
        }


        private async Task SetForbiddenImage(HttpContext context)
        {
            var defaultImagePath = "wwwroot/Image/000-404.png";
            var path = Path.Combine(Directory.GetCurrentDirectory(), defaultImagePath);

            FileStream fileStream = File.OpenRead(path);
            byte[] bytes = new byte[fileStream.Length];
            await fileStream.ReadAsync(bytes, 0, bytes.Length);
            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
