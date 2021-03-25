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
            
            var hostBuilder = CreateHostBuilder(args);//����������

            var host = hostBuilder.Build();//���������ɹ�

            host.Run();//׼��һ��web������Ȼ����������

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)//����Ĭ��builderһ����ɸ�������

                .ConfigureWebHostDefaults(//ָ��һ��web������ ------kestrel
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
