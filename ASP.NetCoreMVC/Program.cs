using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASP.NetCoreMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var hostBuilder = CreateHostBuilder(args);//主机建造者

            var host = hostBuilder.Build();//建造主机成功

            host.Run();//准备一个web服务器然后运行起来

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)//创建默认builder一会完成各种配置

                .ConfigureWebHostDefaults(//指定一个web服务器 ------kestrel
                webBuilder =>
                {
                    //webBuilder.UseKestrel((builderContext, options) =>
                    //{
                    //    options.Configure(builderContext.Configuration.GetSection("Kestrel"), reloadOnChange: true);
                    //})
                   webBuilder.UseStartup<Startup>();
                });
    }
}
