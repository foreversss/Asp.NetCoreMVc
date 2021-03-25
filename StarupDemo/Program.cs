using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StarupDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //注册了应用程序必要的几个组件,比如说：配置的组件，容器组件
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //配置应用程序，启动时必要的配置，比如说我们应用程序启动时所需要监听的端口，监听url地址这些
                .ConfigureHostConfiguration(builder =>
                {

                })
                //嵌入我们自己的配置文件，供应用程序来读取，那这些配置将来就会在后续的应用程序执行过程中间每个组件读取
                .ConfigureAppConfiguration(configure => { 
                
                })
                //往容器里面注入应用程序的组件
                .ConfigureServices(service => { 
                
                });
    }
}
