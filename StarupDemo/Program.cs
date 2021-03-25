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
                //ע����Ӧ�ó����Ҫ�ļ������,����˵�����õ�������������
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //����Ӧ�ó�������ʱ��Ҫ�����ã�����˵����Ӧ�ó�������ʱ����Ҫ�����Ķ˿ڣ�����url��ַ��Щ
                .ConfigureHostConfiguration(builder =>
                {

                })
                //Ƕ�������Լ��������ļ�����Ӧ�ó�������ȡ������Щ���ý����ͻ��ں�����Ӧ�ó���ִ�й����м�ÿ�������ȡ
                .ConfigureAppConfiguration(configure => { 
                
                })
                //����������ע��Ӧ�ó�������
                .ConfigureServices(service => { 
                
                });
    }
}
